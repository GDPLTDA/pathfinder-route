import React from 'react'

export default class SearchRoute extends React.Component {
  render() {
    return (
        <div>
            <div className="form-group">
                <button className="btn btn-success" onClick={this.props.Search}>Calculate</button>
            </div> 
        </div>
    )
  }
}
