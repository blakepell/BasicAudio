/*
 * @author            : Blake Pell
 * @initial date      : 2007-03-31
 * @last updated      : 2021-05-02
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using BasicAudio.Extensions;

namespace BasicAudio
{
    /// <summary>
    /// Recording via the current audio device
    /// </summary>
    /// <remarks>
    /// This has been updated to work with Vista/8/10 updated mciSendString APIs (the legacy call
    /// continued to work but would only record 8bit 11KBPS which is horrible quality, had to add
    /// the alignment, samplespersec and bytespersec in that particular order to get it to work
    /// as per the only single post I could find on the web that had content that fixed the issue).
    /// If any latency issues occur, look into whether your sound card supports ASIO drivers.
    /// </remarks>
    public class Recording
    {
        /// <summary>
        /// The bits per second value
        /// </summary>
        public enum BitsPerSampleValue
        {
            Low = 8,
            High = 16
        }

        /// <summary>
        /// Channel settings.  Mono and Stereo are the only supported at this time.
        /// </summary>
        public enum ChannelValue
        {
            Mono = 1,
            Stereo = 2
        }

        /// <summary>
        /// Samples per second values.  The current supported values are 11025, 22050 and 44100 (Low, Medium and High)
        /// </summary>
        public enum SamplesPerSecValue
        {
            Low = 11025,
            Medium = 22050,
            High = 44100
        }

        /// <summary>
        /// Stopwatch for the elapsed time that a recording has taken place.
        /// </summary>
        private readonly Stopwatch _timeElapsed = new Stopwatch();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wavFileName">This is the file that will be saved when the StopRecording method is called.</param>
        public Recording(string wavFileName)
        {
            this.Filename = wavFileName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Recording()
        {
        }

        private bool _isPaused;

        /// <summary>
        /// Gets or sets whether the currently recording is paused or not.
        /// </summary>
        public bool Pause
        {
            get => _isPaused;
            set
            {
                if (this.IsRecording == false)
                {
                    _isPaused = false;

                    return;
                }

                _isPaused = value;

                if (_isPaused)
                {
                    mciSendString("pause capture", null, 0, IntPtr.Zero);
                    _timeElapsed.Stop();
                }
                else
                {
                    mciSendString("resume capture", null, 0, IntPtr.Zero);
                    _timeElapsed.Start();
                }
            }
        }

        /// <summary>
        /// The channel property.  The default value is stereo.
        /// </summary>
        public ChannelValue Channels { get; set; } = ChannelValue.Stereo;

        /// <summary>
        /// The samples per second property.  The default value is high or 44100 samples per second (CD Quality).
        /// </summary>
        public SamplesPerSecValue SamplesPerSecond { get; set; } = SamplesPerSecValue.High;

        /// <summary>
        /// The bits per second property.  The default value is high or 16-bit.
        /// </summary>
        public BitsPerSampleValue BitsPerSample { get; set; } = BitsPerSampleValue.High;

        private string _filename = "";

        /// <summary>
        /// The filename property.  This is the file that will be saved to whenever the StopRecording method is called.  If the
        /// ForceExtension property is set to true (which it is be default) then it will force the filename to have the proper .wav
        /// extension.
        /// </summary>
        public string Filename
        {
            get => _filename;
            set
            {
                // Check if we should force putting the .wav extension on the file, but allow the user
                // to disable this if they want
                if (this.ForceExtension)
                {
                    if ((value.Length > 4) & (value.ToLower().Right(4) != ".wav"))
                    {
                        value = $"{value}.wav";
                    }
                }

                _filename = value;
            }
        }

        /// <summary>
        /// A property that determines whether the class will force the file to have the .wav extension (it will add it if you don't).  The default
        /// value for this is true.
        /// </summary>
        public bool ForceExtension { get; set; } = true;

        /// <summary>
        /// The time elapsed on the current recording.
        /// </summary>
        public long TimeElapsed => _timeElapsed.ElapsedMilliseconds;

        /// <summary>
        /// The current number of bytes that are stored in memory.  This property obtains it's value by making a call to the
        /// Windows API mciSendString.
        /// </summary>
        public long BytesInMemory
        {
            get
            {
                var sb = new StringBuilder(128);
                mciSendString("set capture time format bytes", null, 0, IntPtr.Zero);
                mciSendString("status capture length", sb, 255, IntPtr.Zero);

                try
                {
                    return long.Parse(sb.ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Whether or not the class is currently recording.
        /// </summary>
        public bool IsRecording { get; set; }

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        [DllImport("winmm.dll")]
        private static extern int waveOutGetNumDevs();

        /// <summary>
        /// Start recording
        /// </summary>
        public void StartRecording()
        {
            // If they're already recording silently exit.
            if (this.IsRecording)
            {
                return;
            }

            //Calculate the average bytes/sec and block alignment
            int averageBytes = (int) this.BitsPerSample * (int) this.Channels * (int) this.SamplesPerSecond / 8;
            int alignment = (int) this.BitsPerSample * (int) this.Channels / 8;

            //Even though MCI's documentation does not mention these need to be set in a certain order, I've found that if
            //the below order is not used, the function will fail.
            string command = $"set capture bitspersample {this.BitsPerSample} channels {this.Channels}";
            command += $" alignment {alignment} samplespersec {this.SamplesPerSecond} bytespersec {averageBytes}";
            command += " format tag pcm wait";

            mciSendString("close capture", null, 0, IntPtr.Zero);
            mciSendString("open new type waveaudio alias capture", null, 0, IntPtr.Zero);
            mciSendString(command, null, 0, IntPtr.Zero);
            mciSendString("record capture", null, 0, IntPtr.Zero);

            this.IsRecording = true;

            _timeElapsed.Reset();
            _timeElapsed.Start();
        }

        /// <summary>
        /// Stop recording.  This will also save the file to disk that was previously specified (or should have been previously specified).
        /// </summary>
        public void StopRecording()
        {
            if (string.IsNullOrEmpty(_filename))
            {
                throw new Exception("No file specified to save to.");
            }

            mciSendString("stop capture", null, 0, IntPtr.Zero);
            mciSendString($"save capture {_filename}", null, 0, IntPtr.Zero);
            mciSendString("close capture", null, 0, IntPtr.Zero);

            this.IsRecording = false;

            _timeElapsed.Stop();
        }

        /// <summary>
        /// Whether or not a sound card exists.
        /// </summary>
        public bool SoundCardExists()
        {
            int waveOutDevices = waveOutGetNumDevs();

            if (waveOutDevices > 0)
            {
                return true;
            }

            return false;
        }
    }
}