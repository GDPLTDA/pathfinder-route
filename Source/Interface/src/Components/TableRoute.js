import React from 'react'
import ReactTable from 'react-table'
import MensagemErro from './MensagemErro'

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
        id: 'saida',
        Header: 'Saída',
        maxWidth:90,
        className:'text-center',    
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        accessor: d => {
            return d.dhSaida
          }
      }, {
        id: 'chegada',
        Header: 'Chegada',
        maxWidth:90,
        className:'text-center',
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        accessor: d => {
            return d.dhChegada
          }
      }, {
        Header: 'KM',
        accessor: 'km',
        className:'text-center',
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        maxWidth: 120
      }, {
        Header: 'Tempo',
        accessor: 'minutos',
        className:'text-center',
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        maxWidth:120
      }, {
        Header: 'Espera',
        accessor: 'chegada.minutosespera',
        className:'text-center',
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        maxWidth:120
      }]

      return this.props.listEntregador.map((item, index) => (
        <div className="div-entregador" key={index}>
            <MensagemErro mensagem = {this.props.mensagem} /> 
            <ReactTable
                data={item.rotas}
                columns={[
                    {
                      Header: "Entregador " + (index + 1),
                      columns: columns
                    }]}
                defaultPageSize={item.rotas.length}
                showPagination={false}
                className="-striped -highlight"
                resizable={false}
            />
        </div>
    ));
  }
}
