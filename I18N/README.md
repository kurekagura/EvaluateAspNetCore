# 多言語化

## RequestCultureProviders

RequestLocalizationOptionsの既定では、RequestCultureProvidersに以下の三つ登録される。

- QueryStringRequestCultureProvider
- CookieRequestCultureProvider
- AcceptLanguageHeaderRequestCultureProvider

## ニュートラル言語をenとする場合

クッキーでニュートラル言語を指定することはできない。そのためAccept-Langauageにjaのあるブラウザでは、en.resxが存在しなければ、ニュートラル言語にいきつく以前にjaが利用されてしまう。そのため、日本語ブラウザに対して、英語選択を機能させるために必ずen.resxを提供する必要がある。

en.resxをマスターとして、.resx（ニュートラル言語）をコピーすることで、この問題を回避する。

```xml
  <ItemGroup>
    <Compile Update="Resx\All.Designer.cs">
      <DesignTime>True</DesignTime>
      <AutoGen>True</AutoGen>
      <DependentUpon>All.resx</DependentUpon>
    </Compile>
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Update="Resx\All.resx">
      <Generator>PublicResXFileCodeGenerator</Generator>
      <LastGenOutput>All.Designer.cs</LastGenOutput>
    </EmbeddedResource>
  </ItemGroup>

  <!-- ビルド前に Resx/All.en.resx を Resx/All.resx へコピー -->
  <Target Name="CopyAllResx" BeforeTargets="Build">
    <Copy SourceFiles="Resx\All.en.resx" DestinationFiles="Resx\All.resx" SkipUnchangedFiles="true" />
  </Target>
```

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
