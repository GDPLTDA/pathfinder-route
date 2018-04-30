import React from 'react'
import moment from 'moment';
import ReactTable from 'react-table'
import MensagemErro from './MensagemErro'
import ResearchRouteButton from './ResearchRouteButton'
import TimePicker from 'rc-time-picker';
import LoadingSpinner from './LoadingSpinner'

let times = []
let selectTimes = []
const format = 'HH:mm';
export default class TableRoute extends React.Component {

  onChange = (index,value) => {
    const time =  value ? value.format(format) : "00:00"
    times[index] = time
    selectTimes[index] = value
}

  render() {
    if(this.props.listEntregador.length === 0)
        return <div/>
    
    const columns = [{
        Header: 'Endereço Saída',
        accessor: 'saida.endereco' // String-based value accessors!
      }, {
        Header: 'Endereço Chegada',
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
      const reloading = this.props.reloading

      return this.props.listEntregador.map((item, index) =>(
        <div className="div-entregador" key={index}>
          { reloading[index] && <LoadingSpinner  />  }
          { !reloading[index] &&
          <div>
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
              Saída <TimePicker
                      showSecond={false}
                      className="form-control"
                      defaultValue={selectTimes[index]}
                      onChange={(value) =>this.onChange(index, value)}
                      format='HH:mm'/>
            </div>
            <div className="col-sm-8">
              <ResearchRouteButton 
                  Research={this.props.research}
                  Rotas={item.rotas}
                  Time={() => {return times[index] == null ? moment().format(format) : times[index]}}
                  Index={index}
                  Label="Proxima Rota"/>
              <MensagemErro mensagem = {this.props.listEntregador[index].mensagem} />
            </div>
          </div>
          </div>}
        </div>
    ));
  }
}
