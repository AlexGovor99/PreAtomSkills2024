import { NextPage } from "next";
import ReportEditForm from "./form";

const ReportEditPage: NextPage = async ({ params }: any) => {
  const response = await fetch(`http://localhost:5002/order/get/${params.reportId}`);
  const data = await response.json();

  return <ReportEditForm data={data} />;
};

export default ReportEditPage;
