using BasicAudio;
using System;
using System.Windows.Forms;
using System.IO;
using AudioRecorder.Extensions;

namespace AudioRecorder
{
    public partial class Main : Form
    {
        private string _applicationName = "Audio Recorder";
        private Recording _audioRecorder = new Recording();
        private AudioPlayer _audioPlayer = new AudioPlayer();

        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load event for the main form/application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = _applicationName;
        }

        /// <summary>
        /// Timer that runs while the application is recording to update the UI with information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecordingTimer_Tick(object sender, EventArgs e)
        {
            if (_audioRecorder.IsRecording == true)
            {
                double elapsed = _audioRecorder.TimeElapsed / 1000;
                lblStatus.Text = $"Recording: {elapsed.ToString().FormatIfNumber(2)} seconds";
                lblBytesInMemory.Text = _audioRecorder.BytesInMemory.ToString().Trim();
            }
        }

        /// <summary>
        /// Timer that runs while the application is playing an audio file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            double elapsed = _audioPlayer.Milliseconds / 1000;
            lblStatus.Text = $"Total Time: {elapsed.ToString().FormatIfNumber(2)} seconds";
            lblBytesInMemory.Text = _audioPlayer.FileSize.ToString().FormatIfNumber(0);
        }

        /// <summary>
        /// Opens an audio file.  This does not auto-play after open.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_audioPlayer.Filename) || !File.Exists(_audioPlayer.Filename))
            {
                _audioPlayer.Close();
                lblFilename.Text = "None";
                this.Text = _applicationName;
            }

            OpenFileDialog1.FileName = "";
            OpenFileDialog1.Multiselect = false;
            OpenFileDialog1.Filter = "*.mp3|*.mp3|*.wav|*.wav";
            OpenFileDialog1.ShowDialog();

            if (!File.Exists(OpenFileDialog1.FileName))
            {
                return;
            }

            _audioPlayer.Filename = OpenFileDialog1.FileName;
            btnPause.CheckState = CheckState.Unchecked;
            this.Text = OpenFileDialog1.FileName;
            lblFilename.Text = OpenFileDialog1.FileName;
        }

        /// <summary>
        /// Asks for a location to save the audio file to and begins recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRecord_Click(object sender, EventArgs e)
        {
            SaveFileDialog1.ShowDialog();

            if (string.IsNullOrWhiteSpace(SaveFileDialog1.FileName))
            {
                return;
            }

            this.Text = SaveFileDialog1.FileName;
            RecordingTimer.Enabled = true;
            _audioRecorder.Filename = SaveFileDialog1.FileName;
            _audioRecorder.StartRecording();
            btnRecord.Enabled = false;
        }

        /// <summary>
        /// Plays the current audio file that is opened or asks to open a new audio file if one has not already
        /// been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_audioPlayer.Filename) || !File.Exists(_audioPlayer.Filename))
            {
                OpenFileDialog1.FileName = "";
                OpenFileDialog1.Multiselect = false;
                OpenFileDialog1.Filter = "*.mp3|*.mp3|*.wav|*.wav";
                OpenFileDialog1.ShowDialog();

                if (string.IsNullOrWhiteSpace(_audioPlayer.Filename) || !File.Exists(_audioPlayer.Filename))
                {
                    return;
                }

                _audioPlayer.Filename = OpenFileDialog1.FileName;
                this.Text = OpenFileDialog1.FileName;
                lblFilename.Text = OpenFileDialog1.FileName;
                btnPause.CheckState = CheckState.Unchecked;
                btnRecord.Enabled = false;
                _audioPlayer.Play();
                PlaybackTimer.Enabled = true;
            }
            else
            {
                btnPause.CheckState = CheckState.Unchecked;
                btnRecord.Enabled = false;
                PlaybackTimer.Enabled = true;
                _audioPlayer.Play();
            }

        }
        
        /// <summary>
        /// Stops the current playing file (if one is).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            _audioPlayer.Stop();
            btnRecord.Enabled = true;
            btnPause.CheckState = CheckState.Unchecked;
            RecordingTimer.Enabled = false;
            PlaybackTimer.Enabled = false;

            if (_audioRecorder.IsRecording == true)
            {
                _audioRecorder.StopRecording();
                _audioPlayer.Filename = _audioRecorder.Filename;
            }

            lblStatus.Text = "Idle";

        }

        /// <summary>
        /// Pauses both the player and/or the recorder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.CheckState == CheckState.Unchecked)
            {
                _audioPlayer.Pause();
                _audioRecorder.Pause = true;
                btnPause.CheckState = CheckState.Checked;
            }
            else
            {
                _audioPlayer.Resume();
                _audioRecorder.Pause = false;
                btnPause.CheckState = CheckState.Unchecked;
            }
        }

    }
}
