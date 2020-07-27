# Isogeo for ArcGis Pro

<p align="center">
  <img src="/Resources/Resources/logo-isogeo.png">
</p>

ArcGis Pro Add-In for [Isogeo](https://www.isogeo.com/), a SaaS software to give an easier access to geodata.
Equivalent of [plugins for ArcMap/QGIS](https://www.isogeo.com/nos-produits/Plugins-Widgets).

[Online documentation is available here](http://help.isogeo.com/arcmap/fr/index.html).

## Purpose

Allow Isogeo users to search for datas in their own and external metadata catalogs and add it to a ArcGis Pro project. Its goal is to improve access to internal and external geodata.

## How does it works

### Technical

It's based on Isogeo API:

* RESTful
* oAuth2 protocol used to authenticate shares

It's fully integrated with ArcGis Pro/ESRI ecosystem:

* ArcGis Pro 2.5
* .NET Framework 4.8

### Features

* [X] Text search among Isogeo shares
* [X] Dynamic filter on keywords, INSPIRE themes, catalog owners, source coordinate system and available links
* [X] Geographic filter from the map canvas bounding box
* [X] Order results by relevance, alphabetic, last updated date (data or metadata), creation date (data or metadata)
* [X] Add the related data directly to the map canvas throught raw data or web services
* [X] Display full metadata information in a separated window
* [X] Save search bookmarks

## Screen captures

| Without any search - Dark theme - French | With some filters - Light theme - English |
|:------------------:|:-----------------:|
| ![Search widget with no filters](/Screenshots/Dark%20theme%20-%20French%20-%20Without%20any%20search.PNG) | ![Search widget with some filters](/Screenshots/Light%20theme%20-%20English%20-%20With%20some%20filters.PNG) |

![Add data to the project](/Screenshots/Dark%20theme%20-%20Add%20data%20to%20the%20project.PNG)
![Add data to the project](/Screenshots/Light%20theme%20-%20Add%20data%20to%20the%20project.PNG)

## Getting started

1. Obtain **Isogeo.AddIn.esriAddinX**
1. Execute Isogeo.AddIn file and validate
2. Run ArcGis Pro and log-in to your account
3. Go to Add-In section of the software
4. Click on "Isogeo"
5. Installation is done

See the documentation:

* en [Français](http://help.isogeo.com/arcmap/fr/index.html)
* in [English](http://help.isogeo.com/arcmap/fr/index.html)
  
## Old versions
Some old versions can be find inside [this folder](/Old%20Versions)
  - You will find Screenshots, installer files and dedicated installation documentation for each version
  - Contains the 0.0.0.1 version (early version created before total rework of Isogeo ArcGis Pro Add-In)
  
## Contributors

- This Isogeo ArcGis Pro Add-In was developed by freelance developer [Vianney DOLEANS](https://github.com/VianneyDoleans) in 2020 as part of an order from Isogeo company, under GPL3 license.
  - [GitHub](https://github.com/VianneyDoleans)
  - [LinkedIn](https://www.linkedin.com/in/vianney-doleans-1158a3121/)
- For the development of this Isogeo ArcGis Pro Add-In, Isogeo ArcMap Add-In made by company [GEOFIT](https://geofit.fr/), under GPL3 license, has been used.
  - [Website](https://geofit.fr/)
  - [LinkedIn](https://www.linkedin.com/company/geofit/?originalSubdomain=fr)

