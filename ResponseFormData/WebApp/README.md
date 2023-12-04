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

## 参考

[Reading JSON and binary data from multipart/form-data sections in ASP.NET Core](https://andrewlock.net/reading-json-and-binary-data-from-multipart-form-data-sections-in-aspnetcore/)

