# Approfondimento Tesi di Laurea

Approfondimento per la tesi di Laurea (Sviluppo di un'applicazione web ASP.NET MVC per la gestione di risorse aziendali) svolto a seguito del tirocinio per studiare l'implementazione di un servizio di geolocalizzazione della posta, utile a studiare la distanza tra mittente e destinatari.

Scritto in C#, JavaScript e HTML.

# Implementazione

![Main](/images/main.png)

**Obiettivo:** studiare l’integrazione di un servizio di geolocalizzazione della posta per analizzare la distanza tra mittente e destinatari

**Passi base:**

* Ricreare la parte del database aziendale utilizzata nel tirocinio sul database didattico;

* Riprogettare la struttura del database, poiché è stato progettato per supportare flussi di dati (la tabella che rappresenta la singola busta non è connessa con quella contenente i dati degli indirizzi).

La riprogettazione è stata fatta aggiungendo una tabella *Indirizzo*, che contiene tutti i dati necessari, compreso il campo PostGIS *geom*.

## Campo Spaziale

Il campo geom è stato inserito alla tabella Indirizzo con il comando:

`SELECT AddGeometryColumn (‘indirizzo’, ‘geom’, 4326, ‘POINT’, 2);`

dove i campi sono rispettivamente *nomeTabella*, *nomeAttributo*, *SRID*, *tipoOggetto*, *dimensione*.

In questo modo viene creato un attributo PostGIS, che rappresenta un punto spaziale. 

Punti che sono stati inseriti con:

`UPDATE indirizzo SET geom = ST_GeomFromText (‘POINT(10.94641 45.476934)’, 4326)
 WHERE id = ‘ind1000’;`

I valori inseriti in questo modo vengono salvati nel database nel formato WKB (Well Known Binary). Per visualizzarli come coordinate si usa la funzione ST_AsText:

`SELECT id, ST_AsText (geom) AS points;`

## Nuova Web App

Gli attributi geografici inseriti sono stati quindi utilizzati in una nuova Applicazione Web ASP.NET: la connessione a PostgreSQL è stata resa possibile dall’estensione di VisualStudio Npgsql e la rappresentazione su mappa (**OpenStreetMaps**) visualizzata e controllata attraverso la libreria Javascript **LeafLet**.

Con questi mezzi è stato possibile raffigurare la posizione dei destinatari:

* selezionando ogni indirizzo singolarmente;

* raggruppando per mittente in comune;

* limitando la selezione nel raggio di 5km dal mittente. 

In quest’ultimo caso la query contiene la condizione:

`WHERE ST_DistanceSphere (i.geom, i2.geom) <= (5 * 1000);`

### Home

![Home](/images/home.PNG)

### Uso marcatore

![Marcatore](/images/home-mark2.PNG)

### Stesso mittente

![Tutti](/images/tutti.PNG)

### Limite distanza

![Entro](/images/entro.PNG)




