document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('pickupLocation');
    if (input) {
        input.addEventListener('input', function () {
            fetch(`/Cars/GetLocations?term=${input.value}`)
                .then(response => response.json())
                .then(data => {
                    const datalist = document.getElementById('locationsList');
                    datalist.innerHTML = '';
                    data.forEach(loc => {
                        const option = document.createElement('option');
                        option.value = loc;
                        datalist.appendChild(option);
                    });
                });
        });
    }
});
