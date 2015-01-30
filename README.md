# BasicAudio
Basic audio is a class library with a test project (audio player/recorder) to faciliate basic audio playing and recording. There are other frameworks available to give you very detailed and complex audio functionality, this one aims to provide only the basic playback / record methods. That being the case, the goal is to keep it simple for those that just want to incorporate playback/recording with minimal code or learning other frameworks.

If you need advanced recording and audio features I highly recommend [NAudio](http://naudio.codeplex.com/).

Basic audio was written in Visual Basic against the .Net Framework and has been tested against Windows Vista, 7, 8 and 8.1 (Desktop). The class itself handles recording and playback via the mciSendString Windows API. The playback features support wave files and mp3 files and the recoding supports wave files. The class library contains 3 classes, one for playback, one for recording and one that is an MCI error messages (there's an official API for this that I'll use in the future). The classes have been kept slim to facilitate ease of use. If you're looking for very detailed recording objects you'll want to consider another framework such as [NAudio](http://naudio.codeplex.com/) but for basic recording tasks this works very well. Note that it records through whatever the currently selected play back device is in windows.  The project currently targets the 4.0 framework but should be compatible back to 2.0 if needed.

## Basic Start Recording Example

```vbnet
    ' There are properties on this object to change the quality recording
    Dim audioRecorder As New Argus.Audio.Recording()
    audioRecorder.Filename = "c:\test.wav"
    audioRecorder.StartRecording()   
```

## Basic Stop Recording Example
```vbnet
    ' File is written out to disk when this is called.  The filename property must already be set.
    audioRecorder.StopRecording()
```
## Basic Playback Example
```vbnet
    Dim audioPlayer As New Argus.Audio.AudioFile()
    audioPlayer.Filename = "c:\test.mp3"
    audioPlayer.Play()    ' Pause and Stop methods available
```
