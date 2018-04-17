import toastr from 'toastr'

const Search = async (listLocations) => {
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
            DhSaida : "11/12/2017 " + store.from + ":00",
            DhLimite : "11/12/2017 " + store.to + ":00",
            Origem :{
                Endereco : store.address
            },
            Destinos : destinos
        }
      
        const response =  await fetch('http://localhost:64880/api/route', {
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
const Research = async (listLocations) => {
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
            DhSaida : "11/12/2017 " + store.from + ":00",
            DhLimite : "11/12/2017 " + store.to + ":00",
            Origem :{
                Endereco : store.address
            },
            Destinos : destinos
        }
      
        const response =  await fetch('http://localhost:64880/api/route', {
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