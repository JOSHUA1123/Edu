const video = document.getElementById('video');
let currentStream;

function stopMediaTracks(stream) {
    stream.getTracks().forEach(track => {
        track.stop();
    });
}
var ss = [];
function gotDevices(mediaDevices) {
    select.innerHTML = '';
    select.appendChild(document.createElement('option'));
    let count = 1;
    mediaDevices.forEach(mediaDevice => {
        if (mediaDevice.kind === 'videoinput') {
            ss.push(mediaDevice.deviceId);
        }
    });
}



$(function () {
    if (typeof currentStream !== 'undefined') {
        stopMediaTracks(currentStream);
    }
    const videoConstraints = {};
    if (select.value === '') {
        videoConstraints.facingMode = 'environment';
    } else {
        videoConstraints.deviceId = { exact: ss[0] };
    }
    const constraints = {
        video: videoConstraints,
        audio: false
    };
    navigator.mediaDevices
      .getUserMedia(constraints)
      .then(stream => {
          currentStream = stream;
          video.srcObject = stream;
          return navigator.mediaDevices.enumerateDevices();
      })
      .then(gotDevices)
      .catch(error => {
          console.error(error);
      });

});

navigator.mediaDevices.enumerateDevices().then(gotDevices);
