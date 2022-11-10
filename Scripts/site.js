// prevent form resubmission
if (window.history.replaceState) {
    window.history.replaceState(null, null, window.location.href);
}

$('#createDirForm').submit((e) => {
    e.preventDefault();
    return false;
})

$('#createDir').click(() => {
    var dirname = $('#dirname').val()

    if (dirname) {
        $.ajax({
            type: "POST",
            url: "/Upload/CreateDir",
            data: { dirname: dirname },
            dataType: "json",
            success: (reponse) => {
                if (reponse) {
                    window.location.reload();
                }
                else {
                    $('#modal').modal('hide');
                }
            }
        })
    }else {
        $('#modal').modal('hide');
    }
})
