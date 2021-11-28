

$(document).ready(function () {
    var d = document.getElementById('impmsg');
    if (d != null) {
        if(d.value != "")
        new PNotify({
            title: '',
            text:d.value ,
            icon: ''
        })
    }
});
   
