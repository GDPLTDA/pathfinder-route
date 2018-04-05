import React from 'react'

export default class LoadingSpinner extends React.Component {
  render() {
    return (
        <div className="col-sm-12 border-top col-xs-offset-2">
            {this.props.mensagem}
        </div>
    )
  }
}
