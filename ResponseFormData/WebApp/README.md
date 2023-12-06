# WebAPIのレスポンスでmultipart/form-dataを戻す

## レスポンスBODY

以下のようになる。

```
--578529a0-5d22-47da-b100-e4f255bfc25f
Content-Type: image/png
Content-Disposition: form-data; name=name1; filename=name1

<バイナリ>

--578529a0-5d22-47da-b100-e4f255bfc25f
Content-Type: application/json
Content-Disposition: form-data; name=name2

{"width":100,"height":200}
--578529a0-5d22-47da-b100-e4f255bfc25f--
```

## checkboxタグヘルパの仕組み

タグヘルパを利用してcheckboxを作成すると、以下のようなHTMLを生成する。

```html
<form>
    <div>
        <input type="checkbox" data-val="true"
            data-val-required="The チェックしてください(LOC可) field is required."
            id="name_MyCheckbox" name="name_MyCheckbox" value="true">
        <label for="name_MyCheckbox">チェックしてください(LOC可)</label>

    </div>
    <input name="name_MyCheckbox" type="hidden" value="false">
</form>
```

必ずform閉じの直前にこのようなhiddenを挿入するため、FormDateで末尾にappendされる。

チェックONとなるとvalue="true"もFormDataに登録されるものの、jsonシリアライズの際、後のvalue="false"が勝ってしま、常にfalseになる。

回避策として、この要素を削除する。

```js
const $form_b = $('.my-plugin-b').find('form:first');
const checkboxes = $form_b.find('input[type="checkbox"]');
checkboxes.each(function () {
    var nameAttr = $(this).attr('name');
    // input type="hidden"かつ同じname属性を持つ要素を削除
    $form_b.find('input[type="hidden"][name="' + nameAttr + '"]').remove();
});
```

## 参考

[Reading JSON and binary data from multipart/form-data sections in ASP.NET Core](https://andrewlock.net/reading-json-and-binary-data-from-multipart-form-data-sections-in-aspnetcore/)

## Memo

```js
// FormDataにinput要素を直接入れることができる？
$form.submit(function (event) {
    // サブミット時にデータを設定
    for (var key in formData) {
        if (formData.hasOwnProperty(key)) {
            $form.append($('<input>', {
                type: 'hidden',
                name: key,
                value: formData[key]
            }));
        }
    }
});
```