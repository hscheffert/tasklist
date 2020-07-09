import React from 'react';
import { Space, notification, Select } from 'antd';
import TaskDTO from '../../models/generated/TaskDTO';
import { EditOutlined, MoreOutlined } from '@ant-design/icons';
import HistoryUtil from '../../utils/HistoryUtil';
import TaskDetailsModal from './TaskDetailsModal';
import TaskApiController from '../../api/TaskApiController';
import StaffTypeApiController from '../../api/StaffTypeApiController';
import UserApiController from 'api/UserApiController';
import StaffTypeDTO from 'models/generated/StaffTypeDTO';
import DataTable, { DataTableColumnProps } from '../shared/DataTable';
import DataTableUtil from '../../utils/DataTableUtil';
import Routes from 'config/ConfigureRoutes';
import { UserState } from 'redux/UserReducer';
import UserDTO from 'models/generated/UserDTO';
import { renderUserSelectOption } from 'utils/FormUtil';

interface TaskTableProps {
  User: UserState;
}

interface TaskTableState {
  loading: boolean;
  data: TaskDTO[];
  modalVisible: boolean,
  selectedTaskId: string | null;
  staffTypes: StaffTypeDTO[];
  users: UserDTO[];
  selectedUserId: string;
}

class TaskTable extends React.Component<TaskTableProps, TaskTableState> {
  private dataTable: DataTable<TaskDTO>;
  private customTableColumns: DataTableColumnProps<any>[] = [
    DataTableUtil.Columns.DisplayOrder(true),
    DataTable.StandardColumns.Text('Name', 'name', { columnProps: { defaultSortOrder: 'descend'}}),
    DataTable.StandardColumns.Text('Area', 'areaName'),
    DataTable.StandardColumns.Text('Sub Area', 'subAreaName'),
    DataTable.StandardColumns.Text('Primary Staff', 'primaryStaffName'),
    DataTable.StandardColumns.Text('Frequency', 'frequencyName'),
  ];

  constructor(props: TaskTableProps) {
    super(props);
    this.state = {
      loading: false,
      data: [],
      modalVisible: false,
      selectedTaskId: null,
      staffTypes: [],
      users: [],
      selectedUserId: null // this.props.User.id
    };
  }

  componentDidMount() {
    this.customTableColumns.push(DataTableUtil.Columns.Active(this.props.User.isAdmin && this.handleActiveToggle));
    this.customTableColumns.push({
      columnProps: {
        title: 'Actions',
        key: 'actions',
        render: (value: any, record: TaskDTO) => {
          const moreIcon = <MoreOutlined onClick={() => this.viewDetails(record.taskId)} />;

          if (this.props.User.isAdmin) {
            return (
              <Space style={{ width: '100%', justifyContent: 'space-between' }}>
                <EditOutlined onClick={() => this.editTask(record.taskId)} />
                {moreIcon}
              </Space>
            );
          }

          return moreIcon;
        }
      }
    });

    this.fetchData();
    this.fetchUsers();
  }

  renderTable = () => {
    const addButton = DataTable.TableButtons.Add("Add Task", Routes.TASK_EDIT('0').ToRoute());

    return (
      <DataTable
        ref={(ele: any) => this.dataTable = ele}
        serverSide={false}
        tableProps={{
          rowKey: 'rowKey',
          loading: this.state.loading,
          sortDirections: ['ascend', 'descend'],
        }}
        columns={this.customTableColumns}
        data={this.state.data}
        globalSearch={true}
        buttonBar={this.props.User.isAdmin && [addButton]}
      />
    );
  }

  renderUserDropdownFilter = () => {
    return (
      <Select
        placeholder="Select Person"
        style={{ width: '200px' }}
        allowClear={true} 
        value={this.state.selectedUserId}
        onChange={this.handleSelectUser}>
        {this.state.users.map(renderUserSelectOption)}
      </Select>
    );
  }

  render() {
    return (
      <Space direction="vertical" style={{ width: '100%' }} size={'small'}>
        {this.renderUserDropdownFilter()}

        {this.renderTable()}

        <TaskDetailsModal
          id={this.state.selectedTaskId as string}
          visible={this.state.modalVisible}
          closeModal={this.closeModal}
          staffTypes={this.state.staffTypes}
        />
      </Space>
    );
  }

  private fetchData = async () => {
    this.setState({ loading: true });   

    try {
      const result = await TaskApiController.getAllUserTasks(this.state.selectedUserId || '');

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

  private fetchStaffTypes = async () => {
    if (this.state.staffTypes.length) {
      return;
    }

    this.setState({ loading: true });
    try {
      const result = await StaffTypeApiController.getAll();

      this.setState({
        loading: false,
        staffTypes: result.data,
      });
    } catch (err) {
      this.setState({ loading: false });
      console.error(err);
      notification.error({
        message: err.message,
        description: err.description
      });
    }
  }

  private fetchUsers = async () => {
    if (this.state.users.length) {
      return;
    }

    this.setState({ loading: true });
    try {
      const result = await UserApiController.getAll();

      this.setState({
        loading: false,
        users: result.data,
      });
    } catch (err) {
      this.setState({ loading: false });
      console.error(err);
      notification.error({
        message: err.message,
        description: err.description
      });
    }
  }

  private handleActiveToggle = async (task: TaskDTO) => {
    if (task.taskId == null) return;

    this.setState({ loading: true });

    try {
      const result = await TaskApiController.toggle(task.taskId);

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

  private handleSelectUser = (value: string) => {
    this.setState({
      selectedUserId: value
    },     
    () => this.fetchData());
  }

  private editTask = (taskId: string | null) => {
    HistoryUtil.push(Routes.TASK_EDIT(taskId as string).ToRoute());
  }

  private viewDetails = async (taskId: string | null) => {
    if (!this.state.staffTypes.length) {
      await this.fetchStaffTypes();
    }

    this.setState({
      modalVisible: true,
      selectedTaskId: taskId
    });
  }

  private closeModal = () => {
    this.setState({
      modalVisible: false,
      selectedTaskId: null
    });
  }
}

export default TaskTable;
