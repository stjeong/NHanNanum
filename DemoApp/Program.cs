using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using kr.ac.kaist.swrc.jhannanum.comm;
using kr.ac.kaist.swrc.jhannanum.hannanum;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Workflow workflow = WorkflowFactory.getPredefinedWorkflow(WorkflowFactory.WORKFLOW_NOUN_EXTRACTOR);

            try
            {
                /* Activate the work flow in the thread mode */
                workflow.activateWorkflow(true);

                string document = File.ReadAllText("sample.txt");
                StringBuilder sb = new StringBuilder();

                /* Analysis using the work flow */
                workflow.analyze(document);

                LinkedList<Sentence> resultList = workflow.getResultOfDocument(new Sentence(0, 0, false));
                foreach (Sentence s in resultList)
                {
                    Eojeol[] eojeolArray = s.Eojeols;
                    for (int i = 0; i < eojeolArray.Length; i++)
                    {
                        if (eojeolArray[i].length > 0)
                        {
                            String[] morphemes = eojeolArray[i].Morphemes;
                            for (int j = 0; j < morphemes.Length; j++)
                            {
                                sb.Append(morphemes[j]);
                                sb.Append(", ");
                            //    Trace.Write(morphemes[j]);
                            }
                            //Trace.Write(", ");
                        }
                    }
                }

                File.WriteAllText("sample.output.txt", sb.ToString());

                workflow.close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }

            /* Shutdown the work flow */
            workflow.close();
        }
    }
}
