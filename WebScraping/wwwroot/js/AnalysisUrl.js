
$(document).ready(function () {

    $("body").on("click", "img", function () {


        html = `
<div style="background-color:black;display: block;width: 86%;overflow-wrap: break-word;margin: 7%;">
    <span>Alt:"${($(this).attr("alt") == null ? "" : $(this).attr("alt"))}"       Src:"${$(this).attr("src")}"</span>
</div>
<img style="max-width: 100%;max-height: 100vh;/* position: fixed; */left: 0;background: black;" src="${$(this).attr("src")}">`
        $("#fullImage").html(html)


        $("#imgModal").modal("toggle")
    })

    $("#imgModal").on("click", function (e){
        $("#imgModal").modal("toggle")
    })

    $("#frmUrl").on("submit", function (e) {
        e.preventDefault();
        $('#ajaxModal').modal('show');
        if (typeof (chart) != "undefined") {
            chart.destroy()
        }
        $("#ImagesArea").html("")

        var url = `${$(this).attr("action")}?Url=${$("#Url").val()}`
        $.ajax({
            url: url,
            dataType: "JSON",
            method: $(this).attr("method"),
            success: function (data) {
                if (data.success) {
                    const Words = [];
                    const Quantity = []

                    for (i = 0; i < 10; i++) {
                        Words[i] = data.data.analysisModel.words[i].value
                        Quantity[i] = data.data.analysisModel.words[i].quantity
                    }

                    const dataSet = {
                        labels: Words,
                        datasets: [{
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.6)',
                                'rgba(255, 159, 64, 0.6)',
                                'rgba(255, 205, 86, 0.6)',
                                'rgba(75, 192, 192, 0.6)',
                                'rgba(54, 162, 235, 0.6)',
                                'rgba(153, 102, 255, 0.6)',
                                'rgba(201, 203, 207, 0.6)'],
                            data: Quantity,
                        }]
                    };

                    var options = {
                        plugins: {
                            legend: {
                                display: false,
                            }
                        },
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }

                    }

                    const config = {
                        type: 'bar',
                        data: dataSet,
                        options
                    };
                    chart = new Chart(
                        $('#Chart'),
                        config
                    );
                    var html = ""
                    for (var i = 0; i < data.data.analysisModel.images.length; i++) {
                        html += `<img onerror="this.style.display='none'" style="2px solid red !important; margin:3px" width="150" height="150" src="${data.data.analysisModel.images[i].url}" alt="${data.data.analysisModel.images[i].alt}">`
                    }
                    $("#ImagesArea").html(html)
                    $('#ajaxModal').modal('hide');

                } else
                {
                    $('#ajaxModal').modal('hide');
                    alert("Error loading url")
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error loading url")
                console.log(jqXHR);
                console.log(errorThrown);
                $('#ajaxModal').modal('hide');
            }
        });


    });
});