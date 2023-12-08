# 多言語化

RequestLocalizationOptionsの既定では、RequestCultureProvidersに以下の三つ登録される。

- QueryStringRequestCultureProvider
- CookieRequestCultureProvider
- AcceptLanguageHeaderRequestCultureProvider

## select要素

組込み型SelectListとTupleを使うのがシンプル。

バインドが不要な場合は選択した値のためのModelのプロパティ宣言をなくせる（Ajax利用目的）。但し、label for="id"のためにidを直に指定する必要がある。

```html
<form>
    <label asp-for="@Model.name_SelectedSelectSeasonItem"></label>
    <select asp-for="@Model.name_SelectedSelectSeasonItem" asp-items="@Model.SelectSeasonItems"></select>
</form>

<form>
    <label asp-for="@Model.SelectSeasonItems2"></label>
    <select id="@nameof(Model.SelectSeasonItems2)" asp-items="@Model.SelectSeasonItems2"></select>
</form>
```

## 深謝

- [Enumに定義してあるDisplay属性を表示する。リソースファイルがある場合、リソースから取得する拡張メソッド](https://qiita.com/mak_in/items/7909e51d249826115403)
