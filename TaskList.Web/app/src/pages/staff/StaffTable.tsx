import React from 'react';
import { Space, notification, Select } from 'antd';
import TaskDetailsDTO from '../../models/generated/TaskDetailsDTO';
import HistoryUtil from '../../utils/HistoryUtil';
import DataTable, { DataTableColumnProps } from '../shared/DataTable';
import Routes from 'config/ConfigureRoutes';
import StaffDTO from 'models/generated/StaffDTO';
import StaffApiController from 'api/StaffApiController';
import TaskStaffDTO from 'models/generated/TaskStaffDTO';
import { EditOutlined, DeleteOutlined } from '@ant-design/icons';

interface StaffTableProps {
  isAdmin: boolean;
  taskId: string;
  task: TaskDetailsDTO;
}

interface StaffTableState {
  loading: boolean;
  data: TaskStaffDTO[];
}

class StaffTable extends React.Component<StaffTableProps, StaffTableState> {
  private dataTable: DataTable<StaffDTO>;
  private customTableColumns: DataTableColumnProps<any>[] = [
    DataTable.StandardColumns.Text('Name', 'name'),
    DataTable.StandardColumns.Text('Type', 'staffTypeName'),
  ];

  constructor(props: StaffTableProps) {
    super(props);
    this.state = {
      loading: false,
      data: this.props.task.taskStaff,
    };
  }

  componentDidMount() {
    this.customTableColumns.push({
      columnProps: {
        title: 'Actions',
        key: 'actions',
        render: (value: any, record: StaffDTO) => {
          const deleteIcon = <DeleteOutlined onClick={() => this.handleDelete(record.staffId)} />;

          if (this.props.isAdmin) {
            return (
              <Space size={20}>
                <EditOutlined onClick={() => this.editStaff(record.staffId)} />
                {deleteIcon}
              </Space>
            );
          }

          return deleteIcon;
        }
      }
    });
    this.fetchData();
  }

  componentDidUpdate(prevProps: StaffTableProps) {
    if (!prevProps.task.taskStaff.length && this.props.task.taskStaff.length) {
      this.dataTable.refresh();
    }
  }

  render() {
    const addButton = DataTable.TableButtons.Add("Add Staff", Routes.STAFF_EDIT('0', this.props.task.taskId).ToRoute());

    return (
      <DataTable
        ref={(ele: any) => this.dataTable = ele}
        serverSide={false}
        tableProps={{
          rowKey: 'staffId',
          loading: this.state.loading,
          sortDirections: ['ascend', 'descend'],
          pagination: false
        }}
        columns={this.customTableColumns}
        data={this.state.data}
        globalSearch={true}
        buttonBar={this.props.isAdmin && [addButton]}
      />
    );
  }

  private fetchData = async () => {
    if (this.props.taskId === '0') return;

    this.setState({ loading: true });

    try {
      const result = await StaffApiController.getAllByTaskId(this.props.taskId);

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

  private editStaff = (staffId: string | null) => {
    HistoryUtil.push(Routes.STAFF_EDIT(staffId as string, this.props.task.taskId).ToRoute());
  }

  private handleDelete = async (staffId: string) => {
    this.setState({ loading: true });

    try {
      const result = await StaffApiController.delete(staffId);

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
}

export default StaffTable;
