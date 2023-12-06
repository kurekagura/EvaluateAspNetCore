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

function postFormDataGetFormDataAsync(url, fd) {
    return new Promise((resolve, reject) => {
        fetch(url, {
            method: 'POST',
            body: fd,
            //必要に応じてヘッダーを設定
            //headers: {
            //    'Authorization': 'Bearer ' + accessToken
            //}
        }).then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.formData();
        }).then(data => {
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
        }).catch(error => {
            console.error('GETエラー:', error);
            reject(error);
        });
    });
}

function getBlobFromUrl(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.responseType = 'blob';

    xhr.onload = function () {
        if (xhr.status === 200) {
            callback(xhr.response);
        }
    };

    xhr.send();
}

function fetchBlobFromUrlAsync(url) {
    return new Promise((resolve, reject) => {
        fetch(url)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.blob();
            })
            .then(blob => {
                resolve(blob);
            })
            .catch(error => {
                reject(error);
            });
    });
}

function deleteUnneededFromFormData(fd) {
    fd.delete('__RequestVerificationToken');
    fd.delete('__Invariant'); //複数ある場合、全部削除できる。
    fd.delete('__hoge');　//存在しなくてもエラーにならない。
}

function formDataToJsonFilter(inputElem) {
    //falseを戻すと対象から外せる
    if ((inputElem.type === 'hidden')) {
        switch (inputElem.name) {
            case '__RequestVerificationToken':
            case '__Invariant':
                console.log(`${inputElem.name}は対象から削除`);
                return false;
            default:
                return true;
        }
    }
    return true;
}