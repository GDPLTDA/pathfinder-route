import React from 'react'

export default class SearchRoute extends React.Component {
  constructor(props) {
    super(props)
  }

  Search = (e) => {
      console.log("Buscando..")
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
