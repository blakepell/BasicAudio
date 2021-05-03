Public Class BasicAudioConsole

    Private _audioRecorder As New BasicAudio.Recording()
    Private _audioPlayer As New BasicAudio.AudioPlayer()

    Private Sub BasicAudioConsole_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecord.Click        
        SaveFileDialog1.ShowDialog()

        If SaveFileDialog1.FileName = "" Then
            Exit Sub
        End If

        Me.Text = SaveFileDialog1.FileName
        RecordingTimer.Enabled = True
        _audioRecorder.Filename = SaveFileDialog1.FileName
        _audioRecorder.StartRecording()
        btnRecord.Enabled = False
    End Sub

    Private Sub btnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlay.Click

        If _audioPlayer.Filename = "" Then
            OpenFileDialog1.FileName = ""
            OpenFileDialog1.Multiselect = False
            OpenFileDialog1.Filter = "*.mp3|*.mp3|*.wav|*.wav"
            OpenFileDialog1.ShowDialog()

            If OpenFileDialog1.FileName = "" Then
                Exit Sub
            End If

            _audioPlayer.Filename = OpenFileDialog1.FileName
            Me.Text = OpenFileDialog1.FileName
            lblFilename.Text = OpenFileDialog1.FileName
            btnPause.CheckState = CheckState.Unchecked
            btnRecord.Enabled = False
            _audioPlayer.Play()
            PlaybackTimer.Enabled = True
        Else
            btnPause.CheckState = CheckState.Unchecked
            btnRecord.Enabled = False
            PlaybackTimer.Enabled = True
            _audioPlayer.Play()
        End If

    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        _audioPlayer.Stop()
        btnRecord.Enabled = True
        btnPause.CheckState = False
        RecordingTimer.Enabled = False
        PlaybackTimer.Enabled = False

        If _audioRecorder.IsRecording = True Then
            _audioRecorder.StopRecording()
            _audioPlayer.Filename = _audioRecorder.Filename
        End If

        lblStatus.Text = "Idle"

    End Sub

    Private Sub btnPause_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPause.Click

        If btnPause.CheckState = CheckState.Unchecked Then
            _audioPlayer.Pause()
            _audioRecorder.Pause = True
            btnPause.CheckState = CheckState.Checked
        Else
            _audioPlayer.Resume()
            _audioRecorder.Pause = False
            btnPause.CheckState = CheckState.Unchecked
        End If

    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click

        If _audioPlayer.Filename <> "" Then
            _audioPlayer.Close()
            lblFilename.Text = "None"
            Me.Text = "Basic Audio Console"
        End If

        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.Filter = "*.mp3|*.mp3|*.wav|*.wav"
        OpenFileDialog1.ShowDialog()

        If OpenFileDialog1.FileName = "" Then
            Exit Sub
        End If

        _audioPlayer.Filename = OpenFileDialog1.FileName
        btnPause.CheckState = CheckState.Unchecked
        Me.Text = OpenFileDialog1.FileName
        lblFilename.Text = OpenFileDialog1.FileName

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordingTimer.Tick

        If _audioRecorder.IsRecording = True Then
            lblStatus.Text = "Recording:  " & FormatNumber(_audioRecorder.TimeElapsed / 1000, 2, TriState.True, TriState.False, TriState.True) & " seconds"
            lblBytesInMemory.Text = FormatNumber(_audioRecorder.BytesInMemory.ToString.Trim, 0, TriState.True, TriState.False, TriState.True)
        End If

    End Sub

    Private Sub PlaybackTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlaybackTimer.Tick
        lblStatus.Text = _audioPlayer.Status.Trim & " Total Time: " & FormatNumber(_audioPlayer.Milliseconds / 1000, 2, TriState.True, TriState.False, TriState.True) & " seconds"
        lblBytesInMemory.Text = FormatNumber(_audioPlayer.FileSize, 0, TriState.True, TriState.False, TriState.True)
    End Sub

End Class
