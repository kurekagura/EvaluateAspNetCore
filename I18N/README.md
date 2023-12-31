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

## 試作①（失敗）

（リソースに"文字列"ではなくC#識別子でアクセスするのに必要となる）All.Designer.csをAll.en.resxから直接生成したいと考え、以下を試したがNGだった。All.resxを不要としたかったが標準の仕組みでは無理のよう(?)。

```xml
  <ItemGroup>
    <EmbeddedResource Update="Resx\All.en.resx">
      <Generator>PublicResXFileCodeGenerator</Generator>
      <LastGenOutput>Resx\All.Designer.cs</LastGenOutput>
    </EmbeddedResource>
  </ItemGroup>
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

## PublicResXFileCodeGenerator

.Designer.csを利用する場合（文字列アクセスではなくC#のメンバ名としてアクセスする場合）、アクセス修飾子はinternalでもOKのよう。

## まとめ

Display属性（データアノテーション）の多言語化には共有リソースを利用する。実装例ではResx/All.*.resx。

Razor Page内の多言語化にはPage個別のリソースを利用する。実装例ではResx/Pagesの配下に対象Pageの名前空間/モデル名に準じて作成している。

【重要】nameof(リソースキー)等で指定したい場合、ニュートラル言語リソースファイルからPublicResXFileCodeGeneratorを使いC#クラスを自動生成しておく。但し、特定言語（実装例ではen）を該当言語なしの際のニュートラル言語としたい場合、.en.resxをコピーしておく必要がある（実装例では.csprojでコピーしている）。

これは、Accept-Language:jaのブラウザ環境では、たとえクッキーにenを指定してもニュートラル言語（英語）にはヒットできず、jaが利用されてしまう（すなわち意図的なen指定ができない）事に対する回避策。

## 深謝

- [Enumに定義してあるDisplay属性を表示する。リソースファイルがある場合、リソースから取得する拡張メソッド](https://qiita.com/mak_in/items/7909e51d249826115403)

- [Multilingual Support and Localisation in ASP.NET Core Web Application with Razor Pages](https://www.youtube.com/watch?v=rRpLIytLtbQ)
