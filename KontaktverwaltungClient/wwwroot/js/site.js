$("#vorname").on("blur", function () {
    var vorname = $(this).val();

    $.get("https://api.genderize.io", { name: vorname }, function (data) {
        let g = data.gender;
        if (g === "male") g = "männlich";
        else if (g === "female") g = "weiblich";
        else g = "unbekannt";

        $("#geschlecht").val(g);
    });
});

$("#plz").on("blur", function () {
    var plz = $(this).val();

    $.get("https://api.zippopotam.us/DE/" + plz, function (data) {
        let ort = data.places[0]["place name"];
        $("#ort").val(ort);
    });
});
