import React from 'react';
import { Space, notification, } from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import AreaDTO from 'models/generated/AreaDTO';
import AreaApiController from 'api/AreaApiController';
import DataTable, { DataTableColumnProps, Filterer } from '../shared/DataTable';
import DataTableUtil from '../../utils/DataTableUtil';
import Routes from 'config/ConfigureRoutes';

interface AreaTableProps {
}

interface AreaTableState {
  loading: boolean;
  data: AreaDTO[];
}

class AreaTable extends React.Component<AreaTableProps, AreaTableState> {
  private dataTable: DataTable<AreaDTO>;
  private customTableColumns: DataTableColumnProps<any>[] = [
    DataTableUtil.Columns.DisplayOrder(true),
    DataTable.StandardColumns.Text('Name', 'name'),
    DataTable.StandardColumns.Text('Sub Areas', 'subAreas', {
      renderDataTransform: (subareas: any, record: any) => {
        return subareas.map((subarea: any) => subarea.name).join(', ');
      }
    } as DataTableColumnProps<any>),
  ];

  constructor(props: AreaTableProps) {
    super(props);
    this.state = {
      loading: false,
      data: [],
    };
  }

  componentDidMount() {
    this.customTableColumns.push(DataTableUtil.Columns.Active(this.handleActiveToggle));
    this.customTableColumns.push(DataTableUtil.Columns.Actions('areaId', this.editArea));
    this.fetchData();
  }

  renderTable = () => {
    const addButton = DataTable.TableButtons.Add("Add Area", Routes.AREA_EDIT('0').ToRoute());

    return (
      <DataTable
        ref={(ele: any) => this.dataTable = ele}
        serverSide={false}
        tableProps={{
          rowKey: 'areaId',
          loading: this.state.loading,
          sortDirections: ['ascend', 'descend'],
        }}
        columns={this.customTableColumns}
        data={this.state.data}
        globalSearch={true}
        buttonBar={[addButton]}
      />
    );
  }

  render() {
    return (
      <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
        {this.renderTable()}
      </Space>
    );
  }

  private fetchData = async () => {
    this.setState({ loading: true });
    try {
      const result = await AreaApiController.getAll();

      this.setState({
        loading: false,
        data: result.data,
      });

      this.dataTable.refresh();
    } catch (err) {
      this.setState({ loading: false });
      console.error(err);
      notification.error({
        message: err.message,
        description: err.description
      });
    }
  }

  private handleActiveToggle = async (area: AreaDTO) => {
    if (area.areaId == null) return;

    this.setState({ loading: true });

    try {
      const result = await AreaApiController.toggle(area.areaId);

      this.setState({ loading: false });
      this.fetchData();
    } catch (err) {
      this.setState({ loading: false });
      console.error(err);
      notification.error({
        message: err.message,
        description: err.description
      });
    }
  }

  private editArea = (areaId: string | null) => {
    HistoryUtil.push(Routes.AREA_EDIT(areaId as string).ToRoute());
  }
}

export default AreaTable;
