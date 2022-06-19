# Rosalina CHANGELOG

## 1.1.0 - ...

### 🚀 Enhancement

* Add support for `kebab-case` properties in UXML. ([c2ef56c](https://github.com/Eastrall/Rosalina/commit/c2ef56c001c913d378907db6a760d28d6a503b64))

### 🐛Bug fixes

* Apply filter to `Assets/` folder to prevent generating bindings for internal unity Uxml files. ([PR #17](https://github.com/Eastrall/Rosalina/pull/17))

## 1.0.3 - 2022-06-05

### 🐛Bug fixes

* Fix error when building a project ; excluding Rosalina from output build and making the tool "Editor-Only" ([3f3acdb](https://github.com/Eastrall/Rosalina/commit/3f3acdb65b5c160fc167ff79b0027644f13c6874))

## 1.0.2 - 2022/05/28

### 🪛 Misc

* Remove all Roslyn related `dll` files and build a single `dll` with all necessary components for code generation.
* Add `netstandard2.0` project with the `Rosalina.Roslyn` assembly project generating the single `dll` for code generation.

## 1.0.1 - 2022/03/05

### 🐛Bug fixes

* Fix `NullReferenceException` when no items are selected and user tries to generate scripts using Rosalina. ([48f5c42e](https://github.com/Eastrall/Rosalina/commit/48f5c42ec84334bfb0bece73a7cf063b43bd7026))

### 🪛 Misc

* Review `Rosalina` generation main entry point.
* Clean menu item code.

## 1.0.0 - 2022/02/01

### ✨ Features 

* Watcher on UXML templates
* Code generation for UI bindings based on UXML templates
* Menu-item to re-generate UI bindings
* Menu-item to generate a UI script
