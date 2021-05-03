# Basic Audio

[![NuGet version (BasicAudio)](https://img.shields.io/badge/nuget-v2020.5.3.1-blue.svg?style=flat-square)](https://www.nuget.org/packages/BasicAudio/)
[![NuGet version (BasicAudio)](https://img.shields.io/github/license/blakepell/basicaudio.svg?style=flat-square)](https://github.com/blakepell/BasicAudio/blob/master/LICENSE)

Basic audio is a class library with a test project (audio player/recorder) to faciliate basic audio 
playing and recording. There are other frameworks available to give you very detailed and complex 
audio functionality, this one aims to provide only the basic playback / record methods or provide light
weight code you can include in your project.  That being the case, the goal is to keep it simple for 
those that just want to incorporate playback/recording with minimal code or learning other frameworks.

If you need advanced recording and audio features I highly recommend [NAudio](https://github.com/naudio/NAudio).

Basic audio was originally written in Visual Basic but is now built off of C#.  The Visual Basic 
version has been left in this project for posterity and is in the 'Legacy Visual Basic Version' folder
in the Solution Explorer.

The library provides its functionality through the mciSendString Windows API and thus binds it desktop
use cases. The playback features support wave files and mp3 files and the recoding supports wave files. 
The class library contains 3 classes, one for playback, one for recording and one that is an MCI 
error messages (there's an official API for this that I'll use in the future). The classes have 
been kept slim to facilitate ease of use. If you're looking for very detailed recording objects 
you'll want to consider another framework such as [NAudio](https://github.com/naudio/NAudio). Note 
that this records through whatever the currently selected recording device is in Windows.

## OS Support

- Windows 10
- Windows 8.1
- Windows 8
- Windows 7
- Windows Vista

## .Net Framework Support

- .NET Standard 2.1
- .NET Standard 2.0
- .NET Standard 1.6
- .NET Standard 1.5
- .NET Standard 1.4
- .NET Standard 1.3
- .NET Framework 4.8
- .NET Framework 4.7.2
- .NET Framework 4.7.1
- .NET Framework 4.7
- .NET Framework 4.6.2
- .NET Framework 4.6.1
- .NET Framework 4.6
- .NET Framework 4.5.2
- .NET Framework 4.5.1
- .NET Framework 4.5
- .NET Framework 4
- .NET Framework 3.5

## Start Recording Example

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
## Stop Recording Example

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
## Playback Example

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
## Stop Recording Example

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
## Troubleshooting

On some systems latency might be an issue due to installed audio drivers.  If this occurs and 
your sound card support ASIO drivers then it might be worth it to research [ASIO for all](http://www.asio4all.org/).  I
have used this on two systems in the past with success in mitigating any lag.
