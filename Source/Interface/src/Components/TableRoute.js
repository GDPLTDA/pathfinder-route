import React from 'react'
import moment from 'moment';
import ReactTable from 'react-table'
import MensagemErro from './MensagemErro'
import ResearchRouteButton from './ResearchRouteButton'
import TimePicker from 'rc-time-picker';


const now = moment();

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
        Header: 'KM',
        accessor: 'km',
        className:'text-center',
        width: 60,
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
      }, {
        id: 'saida',
        Header: 'Saída',
        className:'text-center',
        width: 80,
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        accessor: d => {
            return d.dhSaida
          }
      }, {
        Header: 'Percurso',
        accessor: 'minutos',
        className:'text-center',
        width: 80,
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
      }, {
        Header: 'Espera',
        accessor: 'espera',
        className:'text-center',
        width: 80,
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
      }, {
        id: 'chegada',
        Header: 'Entrada',
        className:'text-center',
        width: 80,
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
        accessor: d => {
            return d.dhChegada
          }
      }, {
        Header: 'Descarga',
        accessor: 'descarga',
        className:'text-center',
        width: 80,
        Cell: row =>( <span className='text-center'>{row.value}</span> ),
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
                resizable={true}
                sortable={false}
            />
          <div className="row">
            <div className="col-sm-4">
              <TimePicker
                      showSecond={false}
                      className="form-control"
                      defaultValue={now}
                      format='HH:mm'/>
            </div>
            <div className="col-sm-8">
              <ResearchRouteButton 
                  Research={this.props.research}
                  Rotas={item.rotas}
                  Index={index}
                  Label="Proxima Rota"/>
            </div>
          </div>
        </div>
    ));
  }
}
