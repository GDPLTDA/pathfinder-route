import React from 'react'

export default class SearchRoute extends React.Component {
  constructor(props) {
    super(props)
  }

  Search = async (e)  => {
      console.log("Buscando..")
      const data = await fetch("http://localhost:64880/api/home", {method:'POST'})
      const json = await data.json()
      console.log(json)
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
