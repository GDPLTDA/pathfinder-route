import React from 'react'

export default class SearchRoute extends React.Component {
  constructor(props) {
    super(props)
  }

<<<<<<< HEAD
  Search = (e) => {
    let json = {
      "DhSaida" : "11/12/2017 10:00:00",
      "DhLimite" : "12/12/2017 10:00:00",
     
      "Origem" :{
              "Endereco" : "Rua Maria Roschel Schunck, 817"
      },
      "Destinos" : [
            {
                      "Endereco" : "Av. Engenheiro Eusébio Stevaux, 823",
                      "DhInicial" : "12:00:00",
                      "DhFinal" : "23:00:00",
                      "MinutosEspera" : 30
            },
            {
                        "Endereco" : "Av. das Nações Unidas, 22540 ",
                        "DhInicial" : "12:00:00",
                        "DhFinal" : "23:00:00",
                        "MinutosEspera" : 30
                      }
              ]
      }

      console.log("Buscando..")

      fetch('http://localhost:64880/api/route', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(json)
      }).then((response) => response.json())
      .then((responseJson) => {
        console.log(responseJson)
      })
=======
  Search = async (e)  => {
      console.log("Buscando..")
      const data = await fetch("http://localhost:64880/api/home", {method:'POST'})
      const json = await data.json()
      console.log(json)
>>>>>>> 438c2a569971decf7b93e6470fe4ed008677a7b8
}

  render() {
    return (
        <div>
            <div className="form-group">
                <button className="btn btn-success" onClick={this.Search}>Calculate</button>
            </div> 
        </div>
    )
  }
}
