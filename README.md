# Cap ou pas Cap ?

[![Dotnet Logo](https://www.vectorlogo.zone/logos/dotnet/dotnet-ar21.svg)](https://dotnet.microsoft.com/en-us/) 
[![Angular Logo](https://www.vectorlogo.zone/logos/angular/angular-icon.svg)](https://angular.io/) 

## Introduction

Périmètre de l'application:

L'application à développer est du type « social ». Elle a pour but de permettre à des utilisateurs de
créer des défis simples et à d'autres utilisateurs de dire ou prouver qu'ils les ont réalisés.
Elle est découpée en deux parties : une API et un front-end.

Description des besoins:
- L'application permettra tout d'abords aux utilisateurs de créer un compte personnel (pseudo et mot
de passe).
- Une fois connecté à l'application l'utilisateur pourra créer un nouveau défi qui consiste en un texte
court de 140 caractères maximum. 
- Les utilisateurs pourront lister les défis en cours soit par popularité (en fonction du nombre de
« like ») soit par date de création. Ils auront la possibilité de « liker » un défi depuis la liste, de
commenter les défis par un texte court de 140 caractères maximum ou encore d’indiquer qu'ils ont
réalisé ce défi.
- Ils auront aussi la possibilité d'afficher ou non les défis marqués comme réalisés dans la liste des
défis.
- Une fois créé le défi n'est pas modifiable mais pourra être supprimé si personne n'a fait de réponse
ou de « like » dessus. Si le défi n'a pas de réponse ni de commentaire mais uniquement des « like »,
il pourra être marqué comme masqué c'est à dire visible uniquement par son auteur (ainsi que les
« like » associés).
- Les likes et les indications de réalisations sont modifiables (mais pas les commentaires).


## Installation
Lancer la solution CapOuPasCap.sln

Installer NodeJs & Angular
```
sudo apt install nodejs npm
cd ./Frontend && npm install && npm run start
```

#### URLs:

| Url      | Details   | Compte Dev| Mot de passe
|----------|-----------|-----------|-----------|
|http://localhost:4200| Frontend Angular|stephanie|azerty
|https://localhost:44305/swagger/index.html| Swagger|
|https://localhost:44305/api| Api C#|
