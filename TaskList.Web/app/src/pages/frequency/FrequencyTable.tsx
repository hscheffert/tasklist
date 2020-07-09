import React from 'react';
import { Space, notification, } from 'antd';
import HistoryUtil from '../../utils/HistoryUtil';
import FrequencyApiController from 'api/FrequencyApiController';
import FrequencyDTO from 'models/generated/FrequencyDTO';
import DataTable, { DataTableColumnProps } from '../shared/DataTable';
import DataTableUtil from '../../utils/DataTableUtil';
import Routes from 'config/ConfigureRoutes';

interface FrequencyTableProps {
}

interface FrequencyTableState {
  loading: boolean;
  data: FrequencyDTO[];
}

class FrequencyTable extends React.Component<FrequencyTableProps, FrequencyTableState> {
  private dataTable: DataTable<FrequencyDTO>;
  private customTableColumns: DataTableColumnProps<any>[] = [
    DataTableUtil.Columns.DisplayOrder(true),
    DataTable.StandardColumns.Text('Name', 'name'),
  ];

  constructor(props: FrequencyTableProps) {
    super(props);
    this.state = {
      loading: false,
      data: [],
    };
  }

  componentDidMount() {
    this.customTableColumns.push(DataTableUtil.Columns.Active(this.handleActiveToggle));
    this.customTableColumns.push(DataTableUtil.Columns.Actions('frequencyId', this.editFrequency)); 
    this.fetchData();
  }

  renderTable = () => {
    const addButton = DataTable.TableButtons.Add("Add Frequency", Routes.FREQUENCY_EDIT('0').ToRoute());

    return (
      <DataTable
        ref={(ele: any) => this.dataTable = ele}
        serverSide={false}
        tableProps={{
          rowKey: 'frequencyId',
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
      const result = await FrequencyApiController.getAll();

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

  private handleActiveToggle = async (frequency: FrequencyDTO) => {
    if (frequency.frequencyId == null) return;

    this.setState({ loading: true });

    try {
      const result = await FrequencyApiController.toggle(frequency.frequencyId);

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

  private editFrequency = (frequencyId: string | null) => {
    HistoryUtil.push(Routes.FREQUENCY_EDIT(frequencyId).ToRoute());
  }
}

export default FrequencyTable;
