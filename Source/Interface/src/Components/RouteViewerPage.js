
import React from 'react'
import TableRoute from './TableRoute'
import LoadingSpinner from './LoadingSpinner'
import Map from './Map'

export default class RouteViewerPage extends React.Component
{
    constructor() 
    {
        super()
        this.state = {index:0}
    }

    changeIndex = ({target}) => this.setState({index: target.value})


    onMapMounted = (mapRef) => {
        this.setState({ mapRef: mapRef });
    }

    render () {
            const loading = this.props.loading
            const results = this.props.results

            return (
                <div className="form-group row app">
                    { loading && <LoadingSpinner  />  }
                    { !loading && 
                    <div className="col-sm-8"> 
                    <TableRoute 
                        listEntregador = {results}
                        reloading = {this.props.reloading}
                        research={this.props.research}/> 
                    </div> }
                    { !loading &&
                        <div className="col-sm-4">
                            <div className="form-group">
                            <label htmlFor="indexSelect">Entregador:</label>
                            <select className="form-control" name="indexSelect" value={this.state.value} onChange={this.changeIndex}>
                                {results.map((e,i)=><option key={i} value={i}>{i+1}</option>)}
                            </select>
                            </div>
                            <Map
                                listEntregador = {results}
                                currentIndex = {this.state.index}
                                zoom = {12}
                                ref={this.onMapMounted}
                                loadingElement={<div style={{ height: `100%` }} />}
                                containerElement={<div style={{ height: `100%` }} />}
                                mapElement={<div style={{ height: `100%` }} />}
                                googleMapURL={"https://maps.googleapis.com/maps/api/js?key=AIzaSyBm6unznpnoVDNak1s-iV_N9bQqCVpmKpE&v=3.exp&libraries=geometry,drawing,places"}
                            />
                        </div>
                    }
                </div>
            )
    }
}
