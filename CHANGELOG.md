# Rosalina CHANGELOG

## 1.0.3 - 2022-06-05

### 🐛Bug fixes

* Fix error when building a project ; excluding Rosalina from output build and making the tool "Editor-Only" (https://github.com/Eastrall/Rosalina/commit/3f3acdb65b5c160fc167ff79b0027644f13c6874)

## 1.0.2 - 2022/05/28

### 🪛 Misc

* Remove all Roslyn related `dll` files and build a single `dll` with all necessary components for code generation.
* Add `netstandard2.0` project with the `Rosalina.Roslyn` assembly project generating the single `dll` for code generation.

## 1.0.1 - 2022/03/05

### 🐛Bug fixes

* Fix `NullReferenceException` when no items are selected and user tries to generate scripts using Rosalina. (https://github.com/Eastrall/Rosalina/commit/48f5c42ec84334bfb0bece73a7cf063b43bd7026)

### 🪛 Misc

* Review `Rosalina` generation main entry point.
* Clean menu item code.

## 1.0.0 - 2022/02/01

### Features 

* Watcher on UXML templates
* Code generation for UI bindings based on UXML templates
* Menu-item to re-generate UI bindings
* Menu-item to generate a UI script
