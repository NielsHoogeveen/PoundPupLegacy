function initGoogleMaps(id, locations, centerLatitude, centerLongitude) {

        var elem = document.getElementById(id);
        if (elem == null)
            return;
        const map = new google.maps.Map(elem, {
            zoom: 10,
            center: new google.maps.LatLng(centerLatitude, centerLongitude),
            mapTypeId: google.maps.MapTypeId.ROADMAP
        });
        const infowindow = new google.maps.InfoWindow();

        var marker, i;

        for (i = 0; i < locations.length; i++) {
            console.log(locations[i][0]);
            console.log(locations[i][1]);
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(locations[i][0], locations[i][1]),
                map: map
            });

            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    infowindow.setContent(locations[i][0]);
                    infowindow.open(map, marker);
                }
            })(marker, i));
        }
}
function initMap() {

}
function reloadNode(id) {
    window.location = '/node/' + id;
}

function setClickEventHandlersForTerms() {
    document.querySelectorAll('.term-name-label').forEach(item => {
        item.addEventListener('click', event => {
            console.log('clicked');
            var elem = document.getElementById(item.getAttribute("for"));
            if (elem.hasAttribute("checked")) {
                elem.removeAttribute("checked");
            }
            else {
                elem.setAttribute("checked", "on");
            }
            console.log(document.getElementById('term-form').elements);
            var terms = Array.from(document.querySelectorAll('.term-name input[type="checkbox"]')).filter(x => x.hasAttribute('checked')).reduce((accumulator, currentValue) => accumulator + '&' + currentValue.id + '=on', '');
            var url = document.location.origin + document.location.pathname + '?' + terms;
            document.location = url;
            console.log(url);
            console.log(document.location);
            //document.getElementById('term-form').submit();
        })
    })
}

function setClickEventHandlersForSearchForm() {
    document.querySelectorAll('.selection-item-selector').forEach(item => {
        item.addEventListener('change', event => {
            document.getElementById('search-form').submit();
        })
    })
}

function closeHamburgerMenu() {
    document.getElementById('hamburger-button').checked = false;
}

function scrollToFirstError(id) {
    var elem = document.getElementById(id);
    if (elem != null) {
        elem.parentElement.scrollIntoView();
    }
}

async function initMap(locations) {
    // Request needed libraries.
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
    const map = new Map(document.getElementById("map"), {
        center: { lat: 38.0, lng: -95.5 },
        zoom: 4.70,
        mapId: "4504f8b37365c3d0",
    });
    for (i = 0; i < locations.length; i++) {

        const absuseCase = document.createElement("div");

        absuseCase.className = "abuse-case-marker";
        absuseCase.innerHTML = locations[i][3];
        console.log(locations[i][0]);
        console.log(locations[i][1]);

        const marker = new AdvancedMarkerElement({
            map,
            position: new google.maps.LatLng(locations[i][0], locations[i][1]),
            content: absuseCase,
        });
    } 
}