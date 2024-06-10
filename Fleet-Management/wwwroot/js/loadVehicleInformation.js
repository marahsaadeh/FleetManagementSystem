document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/VehicleInformation')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(responseData => {
            // عناصر HTML
            const fleetTableBody = document.querySelector('#vehicleInfoTable tbody');
            const jsonDataDisplay = document.querySelector('#jsonDataDisplay');

            // بيانات المركبات من JSON
            const vehicleData = responseData.DicOfDT.VehicleInformations;

            // ملء جدول بيانات Fleet Vehicle Info
            if (Array.isArray(vehicleData)) {
                vehicleData.forEach(item => {
                    // معالجة `LastPosition` مباشرةً
                    let lastPosition = 'N/A';
                    if (item.LastPosition !== null && item.LastPosition !== "null") {
                        lastPosition = item.LastPosition;
                    }

                    // معالجة `LastGPSSpeed` و `LastAddress` و `LastGPSTime`
                    const lastGPSSpeed = item.LastGPSSpeed || 'N/A';
                    const lastAddress = item.LastAddress || 'N/A';
                    const lastGPSTime = item.LastGPSTime ? new Date(item.LastGPSTime).toLocaleString() : 'N/A';

                    // إنشاء صف لجدول Fleet Vehicle Info
                    const row = document.createElement('tr');
                    row.innerHTML = `
                                           <td>${item.VehicleID}</td>
                                           <td>${item.DriverName}</td>
                                           <td>${item.VehicleMake}</td>
                                           <td>${item.VehicleModel}</td>
                                           <td>${new Date(item.PurchaseDate).toLocaleDateString()}</td>
                                           <td>${lastGPSSpeed}</td>
                                           <td>${lastAddress}</td>
                                           <td>${lastGPSTime}</td>
                                           <td>${lastPosition}</td>
                                       `;
                    fleetTableBody.appendChild(row);
                });
            } else {
                console.error('Expected data to be an array, received:', typeof vehicleData);
                alert('Data format error: Expected an array of vehicle information.');
            }

            // عرض JSON بالكامل في جدول المقارنة
            jsonDataDisplay.textContent = JSON.stringify(responseData, null, 2);
        })
        .catch(error => {
            console.error('Error:', error);
            alert('There was an error fetching the vehicle data.');
        });
});