# ServiceVelibSOAP

Il s'agit d'un WebService .NET qui traduit le service **REST** de l'API Velib JCDecaux en un WebService **SOAP**.

## Extensions :

**Dev :**
-   Client GUI  
-   Accès asynchrones (IWS et client GUI)
-   Cache (IWS)

**Monitoring :**
 - WS Endpoint de monitoring
 - Client GUI de monitoring   

# API Velib

> Toutes les méthodes de l'API sont disponibles en version **Async** renvoyant une Task et en effectuant des appels asyncrones au WebService **REST** ou en utilisant leur **système de cache**.

## GetCities

Récupère **la liste des villes** partenaires de JCDeceaux disposant de stations Velib

  **Requête**

    <GetCities xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Velib"/>

**Réponse**

    <GetCitiesResponse xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Velib/">
      <GetCitiesResult xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:string>CITY1</a:string>
        <a:string>CITY2</a:string>
        <a:string>CITY3</a:string>
        ...
      </GetCitiesResult>
    </GetCitiesResponse>

## GetStations

Récupère **la liste des stations Vélib** d'une ville passée en paramètre, vide si la ville n'existe pas.

  **Requête**
  
	<GetStations xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Velib/">
          <city>CITYNAME</city>
    </GetStations>

**Réponse**

    <GetStationsResponse xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Velib/">
        <GetStationsResult xmlns:a="http://schemas.datacontract.org/2004/07/VelibLibrary" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            <a:Station>
              <a:Address>String Adress</a:Address>
              <a:AvailableStands>Int Stands</a:AvailableStands>
              <a:City>String cityName</a:City>
              <a:Latitude>Double Latitude</a:Latitude>
              <a:Longitude>Double Longitude</a:Longitude>
              <a:Name>String StationName</a:Name>
              <a:Number>Int StationNumber</a:Number>
              <a:Status>String Status</a:Status>
              <a:TotalStands>Int TotalStands</a:TotalStands>
            </a:Station>
            ...
        </GetStationResult>
    </GetStationsResponse>

# API Metrics

## GetMetrics 

Renvoie des informations sur les **durées d'exécution du WebService** (Min,Moyenne,Max)

**Requête**

      <GetMetrics xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Metrics/"/>

**Réponse**
   

    <GetMetricsResponse xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Metrics/">
      <GetMetricsResult xmlns:a="http://schemas.datacontract.org/2004/07/VelibLibrary" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:Avg>Long Average Exectime</a:Avg>
        <a:Max>Long Max Exectime</a:Max>
        <a:Min>Long Min Exectime</a:Min>
      </GetMetricsResult>
    </GetMetricsResponse>

## GetWSRequests

Renvoie le nombre de requêtes effectuées avec l'appel **au service REST** et le nombre de requêtes effectuées en utilisant **la mise en cache** depuis que le WebService est lancé

**Requête**

    <GetWSRequests xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Metrics/" />
    
**Réponse**

    <GetWSRequestsResponse xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Metrics/">
      <GetWSRequestsResult xmlns:a="http://schemas.datacontract.org/2004/07/VelibLibrary" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:Cached>Int NumberofCachedrequests</a:Cached>
        <a:Normal>Int NumberofRESTrequests</a:Normal>
      </GetWSRequestsResult>
    </GetWSRequestsResponse>

## GetWSRequestsTimeSpan

Renvoie le nombre de requêtes effectuées avec l'appel **au service REST** et le nombre de requêtes effectuées en utilisant **la mise en cache** depuis que le WebService est lancé, le tout **dans le laps de temps fourni**

**Requête**

    <GetWSRequestsTimeSpan xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Metrics/">
      <beg>DateTime begin</beg>
      <end>DateTime end</end>
    </GetWSRequestsTimeSpan>
    
**Réponse**

    <GetWSRequestsTimeSpanResponse xmlns="http://ServiceVelibSOAP.com/VelibLibrary/Metrics/">
      <GetWSRequestsResult xmlns:a="http://schemas.datacontract.org/2004/07/VelibLibrary" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
        <a:Cached>Int NumberofCachedrequests</a:Cached>
        <a:Normal>Int NumberofRESTrequests</a:Normal>
      </GetWSRequestsTimeSpanResult>
    </GetWSRequestsTimeSpanResponse>

# Clients 

## Client console

Le projet dispose d'un client console **Console** qui permet les opérations suivantes :

 - ***Help*** : Plus d'informations
 - ***Cities*** : Procure la liste des villes
 - ***Stations*** : Donne des informations sensibles sur toutes les stations Velib d'une ville donnée
 - ***Exit*** : Quitte le client

## Client GUI

Le client **GUI** se décompose en trois parties :

 1.  **La liste des villes disponibles** récupérées auprès du WebService au lancement de l'application
 2.  **La liste des stations d'une ville sélectionnée** dans la liste précédente. obtenue via une requête **asynchrone** au WebService. Elle est mise en cache tant qu'on ne change pas de ville.
 3. **Les informations sur la station** ainsi qu'**une carte montrant son emplacement** ( obtenue avec l'API Google Maps). Pas d'appel au Service ici car toutes les informations sur les stations de la ville sont en cache.


## Client Monitoring

La solution a aussi un **client monitoring** qui vient se brancher au endpoint Monitoring du WebService. Il permet d'obtenir les informations suivantes avec le bouton Refresh.

 - **Le nombre de requêtes effectuées par le WebService.** D'une part les requêtes demandant une **connexion** au service REST et d'autre part celles effectuées grâce au **cache** du WebService. On peut éventuellement spécifié une période (champ pour indiquer qu'on veut les requêtes effectués seulement sur les n dernière minutes)

- **Temps moyen d’exécution** des requêtes par le WebService ainsi que le **minimum** et le **maximum**. Etant donné que le WebService a un cache, le minimum et le maximum peuvent être très hétérogènes.