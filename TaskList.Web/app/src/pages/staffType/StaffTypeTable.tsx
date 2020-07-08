import React from 'react';
import { Space, notification, } from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import StaffTypeDTO from 'models/generated/StaffTypeDTO';
import StaffTypeApiController from 'api/StaffTypeApiController';
import DataTable, { DataTableColumnProps, Renderer, Filterer } from '../shared/DataTable';
import DataTableUtil from '../../utils/DataTableUtil';
import Routes from 'config/ConfigureRoutes';

interface StaffTypeTableProps {
}

interface StaffTypeTableState {
  loading: boolean;
  data: StaffTypeDTO[];
}

class StaffTypeTable extends React.Component<StaffTypeTableProps, StaffTypeTableState> {
  private dataTable: DataTable<StaffTypeDTO>;
  private customTableColumns: DataTableColumnProps<any>[] = [
    DataTable.StandardColumns.Text('Name', 'name'),
    DataTable.StandardColumns.Boolean('Is Supervisor', 'isSupervisor', Filterer.BooleanRadio, Renderer.BooleanYesNo),
    DataTable.StandardColumns.Boolean('Allow Multiple', 'allowMultiple', Filterer.BooleanRadio, Renderer.BooleanYesNo),
  ];

  constructor(props: StaffTypeTableProps) {
    super(props);
    this.state = {
      loading: false,
      data: [],
    };
  }

  componentDidMount() {
    this.customTableColumns.push(DataTableUtil.Columns.Active(this.handleActiveToggle));
    this.customTableColumns.push(DataTableUtil.Columns.Actions('staffTypeId', this.editStaffType));
    this.fetchData();
  }

  renderTable = () => {
    const addButton = DataTable.TableButtons.Add("Add Staff Type", Routes.STAFF_TYPE_EDIT('0').ToRoute());

    return (
      <DataTable
        ref={(ele: any) => this.dataTable = ele}
        serverSide={false}
        tableProps={{
          rowKey: 'staffTypeId',
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
      const result = await StaffTypeApiController.getAll();

      this.setState({
        loading: false,
        data: result.data
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

  private handleActiveToggle = async (staffType: StaffTypeDTO) => {
    if (staffType.staffTypeId == null) return;

    this.setState({ loading: true });

    try {
      const result = await StaffTypeApiController.toggle(staffType.staffTypeId);

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

  private editStaffType = (staffTypeId: string | null) => {
    HistoryUtil.push(Routes.STAFF_TYPE_EDIT(staffTypeId as string).ToRoute());    
  }
}

export default StaffTypeTable;
