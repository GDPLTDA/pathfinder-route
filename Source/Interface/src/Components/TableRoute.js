import React from 'react'
import ReactTable from 'react-table'

export default class TableRoute extends React.Component {
  render() {
    if(this.props.listEntregador.length === 0)
        return <div/>
    
        
    const columns = [{
        Header: 'Saída',
        accessor: 'saida.endereco' // String-based value accessors!
      }, {
        Header: 'Chegada',
        accessor: 'chegada.endereco',
      }, {
        id: 'chegada', // Required because our accessor is not a string
        Header: 'Dh. Chegada',
        accessor: d => {
            const chegada = d.dhChegada
            return chegada
          }
      }, {
        Header: 'Metros',
        accessor: 'metros',
      }, {
        Header: 'Minutos',
        accessor: 'minutos',
      }]
    return (
        <div className="col-sm-12 border-top col-xs-offset-2">
            <h3>Entregador</h3>
            <ReactTable
                data={this.props.listEntregador[0].rotas}
                columns={columns}
            />
        </div>
    )
  }
}
