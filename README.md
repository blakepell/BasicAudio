# BasicAudio
Basic audio is a class library with a test project (audio player/recorder) to faciliate basic audio 
playing and recording. There are other frameworks available to give you very detailed and complex 
audio functionality, this one aims to provide only the basic playback / record methods or provide light
weight code you can include in your project.  That being the case, the goal is to keep it simple for 
those that just want to incorporate playback/recording with minimal code or learning other frameworks.

If you need advanced recording and audio features I highly recommend [NAudio](https://github.com/naudio/NAudio).

Basic audio was originally written in Visual Basic but is now built off of C#.  The Visual Basic version has
been left in this project for posterity.

## OS Support

- Windows 10
- Windows 8.1
- Windows 8
- Windows 7
- Windows Vista

## .Net Framework Support

- .Net Standard 2.0
- .Net Standard 1.6
- .Net Standard 1.5
- .Net Standard 1.4
- .Net Standard 1.3
- .Net Framework 4.7.2
- .Net Framework 4.7.1
- .Net Framework 4.7
- .Net Framework 4.6.2
- .Net Framework 4.6.1
- .Net Framework 4.6
- .Net Framework 4.5.2
- .Net Framework 4.5.1
- .Net Framework 4.5
- .Net Framework 4
- .Net Framework 3.5

These classes provide most of their functionality through the mciSendString Windows API.
The playback features support wave files and mp3 files and the recoding supports wave files. 
The class library contains 3 classes, one for playback, one for recording and one that is an MCI 
error messages (there's an official API for this that I'll use in the future). The classes have 
been kept slim to facilitate ease of use. If you're looking for very detailed recording objects 
you'll want to consider another framework such as [NAudio](https://github.com/naudio/NAudio) but for 
basic recording tasks this works very well. Note that it records through whatever the currently 
selected play back device is in Windows.

## Basic Start Recording Example

##### C#

```csharp
    // There are properties on this object to change the quality recording
    var audioRecorder = new BasicAudio.Recording();
    audioRecorder.Filename = @"c:\test.wav";
    audioRecorder.StartRecording();
```

##### VB.Net

```vbnet
    ' There are properties on this object to change the quality recording
    Dim audioRecorder As New BasicAudio.Recording()
    audioRecorder.Filename = "c:\test.wav"
    audioRecorder.StartRecording()   
```
<br />

## Basic Stop Recording Example

##### C#

```csharp
    audioRecorder.StopRecording();
```

##### VB.Net

```vbnet
    ' File is written out to disk when this is called.  The filename property must already be set.
    audioRecorder.StopRecording()
```

<br />

## Basic Playback Example

##### C#

```csharp
    var audioPlayer = new BasicAudio.AudioPlayer();
    audioPlayer.Filename = @"c:\test.mp3";
    audioPlayer.Play();
```

##### VB.Net

```vbnet
    Dim audioPlayer As New BasicAudio.AudioPlayer()
    audioPlayer.Filename = "c:\test.mp3"
    audioPlayer.Play()    ' Pause and Stop methods available
```

<br />

## Basic Stop Recording Example

##### C#

```csharp
    // File is written out to disk when this is called.  The filename property must already be set.
    audioRecorder.StopRecording();
```

##### VB.Net

```vbnet
    ' File is written out to disk when this is called.  The filename property must already be set.
    audioRecorder.StopRecording()
```
