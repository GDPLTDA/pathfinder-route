import React from 'react'

export default class SearchRouteButton extends React.Component {
  render() {
    return (
        <div>
            <div className="form-group">
                <button className="btn btn-success btn-block" onClick={this.props.Search}>Calculate</button>
            </div> 
        </div>
    )
  }
}
