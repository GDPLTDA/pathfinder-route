import React from 'react'
import { withGoogleMap,GoogleMap, DirectionsRenderer,withScriptjs,Marker } from "react-google-maps"
/* eslint-disable no-undef */

class MapBase extends React.Component {

    constructor(props)
    {
        super(props)
        this.state = {index:0,  center: {lat:0, lng:0}}
    }

   async componentDidMount(){
        if(!this.props.listEntregador)
            return

       const route = this.props.listEntregador[this.state.index].rotas

       console.log(route)
       const directionsPromises = route.map(e => ({
                                         from: { lat: e.saida.lat, lng: e.saida.lng },
                                         to: { lat: e.chegada.lat, lng: e.chegada.lng }
                                     }))
                                .map((r) => this.GetGoogleRoute(r))

       const directions = await Promise.all(directionsPromises)
                   
       console.log(directions)
       this.setState({directions})
    }

    GetGoogleRoute = (points) => new Promise((resolve, reject) => {
        const DirectionsService = new google.maps.DirectionsService()

        DirectionsService.route({
            origin: new google.maps.LatLng(points.from.lat, points.from.lng),
            destination: new google.maps.LatLng(points.to.lat, points.to.lng),
            travelMode: google.maps.TravelMode.DRIVING,
        }, (result, status) => { if (status === google.maps.DirectionsStatus.OK) { resolve(result) } else { reject(result)} });
    })

    render() {

        const center = this.props.center || this.state.center

        return (<GoogleMap
                defaultZoom={this.props.zoom}
                center={new google.maps.LatLng(center.lat, center.lng)} > 
                  {this.state.directions && this.state.directions.map((d,i) => <DirectionsRenderer key={i} directions={d} />)}

                  {this.props.markers && this.props.markers.map((m,i) => <Marker key={i} position={m}/> )}

               </GoogleMap>)
           }
}

const Map = withScriptjs(withGoogleMap(MapBase))
export default Map