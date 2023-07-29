$(function () {
    function searchLocation(searchQuery = "", sorter = "LocationName") {

        if (searchQuery.length == 0) {
            $("caption").html("All locations");
        }
        else {
            $("caption").html("Search results");
        }

        $("tbody").html("");

        let values = searchQuery + "$$" + sorter

        $.ajax({
            type: "POST",
            url: "/api/v1/SearchLocation",
            data: "\"" + values + "\"",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (locations) {
                $.each(locations, function (i, location) {
                    $("tbody").append($("<tr>"));
                    appendElement = $("tbody tr").last();
                    appendElement.append($("<td>").html('<p><a href="/app/locationdetails/' + location.locationId + '" class="link-dark text-decoration-none">' + location.locationName + '</a></p>'));
                    appendElement.append($("<td>").html('<p>' + location.city + '</p>'));
                    appendElement.append($("<td>").html('<p>' + location.phone + '</p>'));
                });
            },
            error: function (xhr, status, error) {
                console.log(xhr)
            }
        });
    }

    $("#txtLocationName").on('keyup', function () {
        var searchQuery = $.trim($("#txtLocationName").val());

        if (searchQuery.length > 1 || searchQuery.length == 0) {
            searchLocation(searchQuery);
        }

    });

    $("#searchButton").on('click', function () {
        var searchQuery = $.trim($("#txtLocationName").val());

        if (searchQuery.length > 1 || searchQuery.length == 0) {
            searchLocation(searchQuery);
        }
    });
    let nameCounter = 1;
    let cityCounter = 0;
    let phoneCounter = 0;
    $("#nameSort").on('click', function () {
        var searchQuery = $.trim($("#txtLocationName").val());

        if (nameCounter == 0) {
            cityCounter = phoneCounter = 0;
            nameCounter++;
            searchLocation(searchQuery);
        }
        else if (nameCounter >= 1) {
            cityCounter = phoneCounter = nameCounter = 0;
            var sorter = "NameDesc";
            searchLocation(searchQuery, sorter);
        }

    });

    $("#citySort").on('click', function () {
        var searchQuery = $.trim($("#txtLocationName").val());
        var sorter = "City";

        if (cityCounter == 0) {
            nameCounter = phoneCounter = 0;
            cityCounter++;
            searchLocation(searchQuery, sorter);
        }
        else if (cityCounter == 1) {
            cityCounter = phoneCounter = nameCounter = 0;
            sorter = "CityDesc";
            searchLocation(searchQuery, sorter);
        }
    });

    $("#phoneSort").on('click', function () {
        var searchQuery = $.trim($("#txtLocationName").val());
        var sorter = "Phone";

        if (phoneCounter == 0) {
            nameCounter = cityCounter = 0;
            phoneCounter++;
            searchLocation(searchQuery, sorter);
        }
        else if (phoneCounter == 1) {
            cityCounter = phoneCounter = nameCounter = 0;
            sorter = "PhoneDesc";
            searchLocation(searchQuery, sorter);
        }
    });
});