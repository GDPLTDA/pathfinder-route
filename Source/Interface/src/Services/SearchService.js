import toastr from 'toastr'

const baseUrl = "https://routega.azurewebsites.net"
const baseDevUrl = "http://localhost:64880"
const apiUrl = `${baseDevUrl}/api/route`


const Search = async (name, entragador, listLocations, config) => {
        const items = listLocations
        const store = items.find( s => s.isStore)
        const listDestinos = items.filter(s => !s.isStore)
        
        const destinos = listDestinos.map(
             d => ({
                Endereco : d.address,
                DhInicial : d.from + ":00",
                DhFinal : d.to + ":00",
                MinutosEspera : d.wait
            })
        )


        const json = {
            Name: name,
            Generations: config.generations,
            Population: config.population,
            Mutation: config.mutation,
            Traffic : config.traffic,
            UseCache: config.useCache,
            NumeroEntregadores: entragador,
            DhSaida : store.from + ":00",
            DhLimite : store.to + ":00",
            Origem :{
                Endereco : store.address
            },
            Destinos : destinos
        }
      
        const response =  await fetch(apiUrl, {
            method: 'POST',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(json)
          })
          .then((response) => response.json())
          .catch( e => {console.log(e); toastr.info(JSON.stringify(e)); throw e})
       
          return response
    }

    const Research = async (listLocations, config) => {
        const items = listLocations
        const store = items.find( s => s.isStore)
        const listDestinos = items.filter(s => !s.isStore)

        const destinos = listDestinos.map(
             d => ({
                Endereco : d.address,
                DhInicial : d.from + ":00",
                DhFinal : d.to + ":00",
                MinutosEspera :d.wait
            })
        )

        const json = {
            Generations: config.generations,
            Population: config.population,
            Mutation: config.mutation,
            Traffic : config.traffic,
            UseCache: config.useCache,
            NumeroEntregadores: 1,
            DhSaida : store.from + ":00",
            DhLimite : store.to + ":00",
            Origem :{
                Endereco : store.address
            },
            Destinos : destinos
        }
      
        const response =  await fetch(apiUrl, {
            method: 'POST',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(json)
          })
          .then((response) => response.json())
          .catch( e => {console.log(e); toastr.info(JSON.stringify(e)); throw e})
       
          return response
    }

export {Search}
export {Research}