/*
 * @author            : Blake Pell
 * @initial date      : 2007-03-31
 * @last updated      : 2021-05-02
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BasicAudio.Extensions;

namespace BasicAudio
{
    /// <summary>
    /// This class is a wrapper for the Windows API calls to play wave, midi or mp3 files.  Although this is .Net Standard 2 compliant
    /// it requires PInvokes to the Windows API and will fail in environments like Linux, Windows Universal Apps, Server environments, etc.
    /// </summary>
    /// <remarks>
    /// This class was originally designed for use in traditional Windows apps long before the invent of .Net Standard.  It is highly
    /// recommended to use a library like NAudio for advanced audio capability.
    /// </remarks>
    public class AudioPlayer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AudioPlayer()
        {
        }

        /// <summary>
        /// Constructor:  Location is the filename of the media to play.  Wave files, mp3 files and midi files are the supported formats.
        /// </summary>
        /// <param name="filePath">File path of the media to play.</param>
        public AudioPlayer(string filePath)
        {
            this.Filename = filePath;
        }

        /// <summary>
        /// Halt the program until the .wav file is done playing.  Be careful, this will lock the entire program up until the
        /// file is done playing.  It behaves as if the Windows Sleep API is called while the file is playing (and maybe it is, I don't
        /// actually know, I'm just theorizing).  :P
        /// </summary>
        public bool Wait { get; set; } = false;

        /// <summary>
        /// Sets the audio file's time format via the mciSendString API.
        /// </summary>
        public int Milliseconds
        {
            get
            {
                var sb = new StringBuilder(255);
                mciSendString("set audiofile time format milliseconds", null, 0, IntPtr.Zero);
                mciSendString("status audiofile length", sb, 255, IntPtr.Zero);

                // Get rid of the nulls
                sb.Replace("\0", "");

                // Get rid of the nulls, they muck things up
                if (string.IsNullOrEmpty(sb.ToString()))
                {
                    return 0;
                }

                return Convert.ToInt32(sb.ToString());
            }
        }

        /// <summary>
        /// Gets the status of the current playback file via the mciSendString API.
        /// </summary>
        public string Status
        {
            get
            {
                var sb = new StringBuilder();
                mciSendString("status audiofile mode", sb, 255, IntPtr.Zero);
                return sb.Replace("\0", "").ToString();
            }
        }

        /// <summary>
        /// Gets the file size of the current audio file.
        /// </summary>
        public long FileSize
        {
            get
            {
                try
                {
                    return new FileInfo(this.Filename).Length;
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Gets the channels of the file via the mciSendString API.
        /// </summary>
        public int Channels
        {
            get
            {
                var sb = new StringBuilder(255);
                mciSendString("status audiofile channels", sb, 255, IntPtr.Zero);

                if (sb.ToString().IsNumeric())
                {
                    return Convert.ToInt32(sb.ToString());
                }

                return -1;
            }
        }

        /// <summary>
        /// Used for debugging purposes.
        /// </summary>
        public string Debug
        {
            get
            {
                var sb = new StringBuilder(255);
                mciSendString("status audiofile channels", sb, 255, IntPtr.Zero);

                return sb.ToString();
            }
        }

        /// <summary>
        /// Whether or not the current playback is paused.
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// The current filename of the file that is to be played back.
        /// </summary>
        public string Filename { get; set; }

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        /// <summary>
        /// Plays the file that is specified as the filename.
        /// </summary>
        public void Play()
        {
            // Some basic checks to make sure a file exists and is available, if not throw an exception the caller
            // can then handle.
            if (string.IsNullOrEmpty(this.Filename))
            {
                throw new FileNotFoundException("No file was provided to Play.");
            }

            if (!File.Exists(this.Filename))
            {
                throw new FileNotFoundException("File does not exist or is inaccessible.");
            }

            // There is probably a better way to do this than to go off of the extension but this is what
            // we're going to do for now unless someone has a need to better identify the file type.
            switch (this.Filename.SafeRight(3).ToLower())
            {
                case "mp3":
                    mciSendString($"open \"{this.Filename}\" type mpegvideo alias audiofile", null, 0, IntPtr.Zero);

                    string playCommand = "play audiofile from 0";

                    if (this.Wait)
                    {
                        playCommand += " wait";
                    }

                    mciSendString(playCommand, null, 0, IntPtr.Zero);

                    break;
                case "wav":
                    mciSendString($"open \"{this.Filename}\" type waveaudio alias audiofile", null, 0, IntPtr.Zero);
                    mciSendString("play audiofile from 0", null, 0, IntPtr.Zero);

                    break;
                case "mid":
                case "idi":
                    var sb = new StringBuilder(128);

                    mciSendString("stop midi", sb, 0, IntPtr.Zero);
                    mciSendString("close midi", sb, 0, IntPtr.Zero);
                    mciSendString($"open sequencer!{this.Filename} alias midi", sb, 0, IntPtr.Zero);
                    mciSendString("play midi", sb, 0, IntPtr.Zero);

                    break;
                default:
                    this.Close();

                    throw new Exception("File type not supported.");
            }

            this.IsPaused = false;
        }

        /// <summary>
        /// Pause the current play back.
        /// </summary>
        public void Pause()
        {
            mciSendString("pause audiofile", null, 0, IntPtr.Zero);
            this.IsPaused = true;
        }

        /// <summary>
        /// Resume the current play back if it is currently paused.
        /// </summary>
        public void Resume()
        {
            mciSendString("resume audiofile", null, 0, IntPtr.Zero);
            this.IsPaused = false;
        }

        /// <summary>
        /// Stop the current file if it's playing.
        /// </summary>
        public void Stop()
        {
            mciSendString("stop audiofile", null, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Close the file.
        /// </summary>
        public void Close()
        {
            mciSendString("close audiofile", null, 0, IntPtr.Zero);
        }
    }
}