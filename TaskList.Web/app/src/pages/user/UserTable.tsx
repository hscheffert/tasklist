import React from 'react';
import { Space, notification, } from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import UserApiController from 'api/UserApiController';
import UserDTO from 'models/generated/UserDTO';
import DataTable, { DataTableColumnProps, Filterer, Renderer } from '../shared/DataTable';
import DataTableUtil from '../../utils/DataTableUtil';
import Routes from 'config/ConfigureRoutes';

interface UserTableProps {
}

interface UserTableState {
  loading: boolean;
  data: UserDTO[];
}

class UserTable extends React.Component<UserTableProps, UserTableState> {
  private dataTable: DataTable<UserDTO>;
  private customTableColumns: DataTableColumnProps<any>[] = [
    DataTable.StandardColumns.Text('Last Name', 'lastName', {
      columnProps: { defaultSortOrder: 'descend' }
    }),
    DataTable.StandardColumns.Text('First Name', 'firstName'),
    DataTable.StandardColumns.Text('Email', 'email'),
    DataTable.StandardColumns.Boolean('Is Supervisor', 'isSupervisor', Filterer.BooleanRadio, Renderer.BooleanYesEmpty),
    DataTable.StandardColumns.Text('Supervisor', 'supervisorName'),
  ];

  constructor(props: UserTableProps) {
    super(props);
    this.state = {
      loading: false,
      data: [],
    };
  }

  componentDidMount() {
    this.customTableColumns.push(DataTableUtil.Columns.Active(this.handleActiveToggle));
    this.customTableColumns.push(DataTableUtil.Columns.Actions('userId', this.editUser));
    this.fetchData();
  }

  renderTable = () => {
    const addButton = DataTable.TableButtons.Add("Add User", Routes.USER_EDIT('0').ToRoute());

    return (
      <DataTable
        ref={(ele: any) => this.dataTable = ele}
        serverSide={false}
        tableProps={{
          rowKey: 'userId',
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
      const result = await UserApiController.getAll();

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

  private handleActiveToggle = async (user: UserDTO) => {
    if (user.userId == null) return;

    this.setState({ loading: true });

    try {
      const result = await UserApiController.toggle(user.userId);

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

  private editUser = (userId: string | null) => {
    HistoryUtil.push(Routes.USER_EDIT(userId).ToRoute());
  }
}

export default UserTable;
