
## Sette opp maskin for kjøring av dokumentasjon
```sh
brew install ruby
sudo gem install jekyll bundler

```

## Kjøre dokumentasjon

```sh
bundle install # Hvis denne feiler, kjør linja under
bundle update # Bør kjøres så ofte som mulig for at visning på Github skal bli korrekt
bundle exec jekyll serve
```

> Får du problemer med skriverettigheter til _/Library/Ruby/*_ så kan det fikses med å ta rettighetene til den mappa: `sudo chown -R $(whoami) /usr/local/`


## For å legge til ny versjon:
* kopier mappen med siste gjeldene versjon og lim det inn som en ny versjon (f.eks. `_v1_0` i root (`cp -r _v1_0 _v1_1`)
* gå inn i index.html, i den nye mappen du lagde, og endre '{% for dok in site.v1_0 %}' til '{% for dok in site.v1_1 %}'
* gå inn i _config.yml og sett riktig versjon i `currentVersion` og `versions`. Legg deretter til den nye versjonen i `collections`.
* I gammel _index.html_ settes `redirect_from:` (`/` fjernes) og i ny _index.html_ settes den til `redirect_from: /`

