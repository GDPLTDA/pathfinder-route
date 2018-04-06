
import React from 'react'
import TableRoute from './TableRoute'
import LoadingSpinner from './LoadingSpinner'
import Map from './Map'

const RouteViewerPage = ({loading, results}) => 
            <div className="form-group row app">

                { loading && <LoadingSpinner  />  }

                { !loading && 
                    <div className="col-sm-6"> <TableRoute  mensagem={results[0].mensagem} listEntregador = {results}/> </div> }

                { !loading &&
                <div className="col-sm-6">
                    <Map
                        listEntregador = {results}
                        zoom = {100}
                        loadingElement={<div style={{ height: `100%` }} />}
                        containerElement={<div style={{ height: `100%` }} />}
                        mapElement={<div style={{ height: `100%` }} />}
                        googleMapURL={"https://maps.googleapis.com/maps/api/js?key=AIzaSyC4R6AN7SmujjPUIGKdyao2Kqitzr1kiRg&v=3.exp&libraries=geometry,drawing,places"}
                    />
                </div>
                }
            </div>


export default RouteViewerPage