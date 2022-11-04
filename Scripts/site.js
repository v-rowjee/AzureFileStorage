$('input[type=file]').change(function () {
    console.dir(this.value);
    console.dir(this.files[0])
})