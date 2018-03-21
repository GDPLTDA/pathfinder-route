import React from 'react'
import ReactTable from 'react-table'

export default class LoadingSpinner extends React.Component {
  render() {
    return (
        <div className="col-sm-12 border-top col-xs-offset-2">
            <i className="fa fa-spinner fa-spin" /> Loading...
        </div>
    )
  }
}
