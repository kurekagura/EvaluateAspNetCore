function getFormDataAsync(url) {
    return new Promise((resolve, reject) => {
        fetch(url)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.formData();
            })
            .then(data => {
                //FormDataからname経由で以下も取り出すことが出来る。
                //Content-Type: image/png
                //Content-Disposition: form - data; name = name1; filename = name1
                console.log('GET成功:', data);
                const jStr = data.get('name2');
                const jObj = JSON.parse(jStr);
                const octet = data.get('name1');
                const contentType = data.get('name1').type;
                const filename = data.get('name1').name;
                console.log(contentType, filename);;
                resolve({ bin: octet, size: jObj, contentType: contentType });
            })
            .catch(error => {
                console.error('GETエラー:', error);
                reject(error);
            });
    });
}
