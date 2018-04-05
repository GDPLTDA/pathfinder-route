
import React from 'react'
import TableRoute from './TableRoute'
import LoadingSpinner from './LoadingSpinner'

const RouteViewerPage = ({loading, results}) => 
<div className="form-group row app">
            {
                loading ? <LoadingSpinner /> : 
                    <TableRoute mensagem={results[0].mensagem} listEntregador = {results}/>
            }
</div>


export default RouteViewerPage